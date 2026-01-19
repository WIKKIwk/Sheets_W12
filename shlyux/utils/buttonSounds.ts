export type ButtonSoundType = 'primary' | 'secondary' | 'danger' | 'toggle' | 'icon';
export type UiSoundType = 'ai-send' | 'ai-receive' | 'file-upload';

type SoundPreset = {
  toneFrequency: number;
  toneFrequencyEnd?: number;
  toneDurationMs: number;
  toneGain: number;
  toneWave: OscillatorType;
  toneDelayMs?: number;
  clickDurationMs: number;
  clickGain: number;
  clickHighpass: number;
};

const BUTTON_PRESETS: Record<ButtonSoundType, SoundPreset> = {
  primary: {
    toneFrequency: 420,
    toneDurationMs: 55,
    toneGain: 0.022,
    toneWave: 'triangle',
    clickDurationMs: 28,
    clickGain: 0.06,
    clickHighpass: 1800,
  },
  secondary: {
    toneFrequency: 360,
    toneDurationMs: 46,
    toneGain: 0.018,
    toneWave: 'triangle',
    clickDurationMs: 24,
    clickGain: 0.05,
    clickHighpass: 1700,
  },
  danger: {
    toneFrequency: 220,
    toneDurationMs: 70,
    toneGain: 0.026,
    toneWave: 'sine',
    clickDurationMs: 30,
    clickGain: 0.065,
    clickHighpass: 1400,
  },
  toggle: {
    toneFrequency: 520,
    toneDurationMs: 38,
    toneGain: 0.016,
    toneWave: 'triangle',
    clickDurationMs: 20,
    clickGain: 0.045,
    clickHighpass: 2100,
  },
  icon: {
    toneFrequency: 460,
    toneDurationMs: 34,
    toneGain: 0.014,
    toneWave: 'triangle',
    clickDurationMs: 18,
    clickGain: 0.04,
    clickHighpass: 2200,
  },
};

const UI_PRESETS: Record<UiSoundType, SoundPreset> = {
  'ai-send': {
    toneFrequency: 620,
    toneFrequencyEnd: 860,
    toneDurationMs: 72,
    toneGain: 0.02,
    toneWave: 'sine',
    toneDelayMs: 4,
    clickDurationMs: 26,
    clickGain: 0.07,
    clickHighpass: 2600,
  },
  'ai-receive': {
    toneFrequency: 880,
    toneFrequencyEnd: 520,
    toneDurationMs: 86,
    toneGain: 0.022,
    toneWave: 'triangle',
    toneDelayMs: 4,
    clickDurationMs: 28,
    clickGain: 0.065,
    clickHighpass: 2300,
  },
  'file-upload': {
    toneFrequency: 320,
    toneFrequencyEnd: 440,
    toneDurationMs: 90,
    toneGain: 0.024,
    toneWave: 'triangle',
    toneDelayMs: 6,
    clickDurationMs: 34,
    clickGain: 0.075,
    clickHighpass: 1500,
  },
};

let audioContext: AudioContext | null = null;
let noiseBuffer: AudioBuffer | null = null;

const getAudioContext = (): AudioContext | null => {
  if (typeof window === 'undefined') return null;

  if (!audioContext) {
    const AudioContextCtor =
      window.AudioContext ||
      (window as Window & { webkitAudioContext?: typeof AudioContext }).webkitAudioContext;

    if (!AudioContextCtor) return null;
    audioContext = new AudioContextCtor();
  }

  return audioContext;
};

export const isSoundEnabled = (): boolean => {
  if (typeof window === 'undefined') return false;
  const stored = localStorage.getItem('app-sound-enabled');
  if (stored === null) return true;
  return stored === 'true';
};

const getNoiseBuffer = (ctx: AudioContext): AudioBuffer => {
  if (noiseBuffer && noiseBuffer.sampleRate === ctx.sampleRate) return noiseBuffer;

  const length = Math.floor(ctx.sampleRate * 0.03);
  const buffer = ctx.createBuffer(1, length, ctx.sampleRate);
  const data = buffer.getChannelData(0);

  for (let i = 0; i < length; i += 1) {
    data[i] = (Math.random() * 2 - 1) * 0.7;
  }

  noiseBuffer = buffer;
  return buffer;
};

const isSoundType = (value: string): value is ButtonSoundType => (
  value === 'primary' ||
  value === 'secondary' ||
  value === 'danger' ||
  value === 'toggle' ||
  value === 'icon'
);

export const getButtonSoundType = (element: HTMLElement): ButtonSoundType => {
  const explicit = element.getAttribute('data-sound');
  if (explicit && isSoundType(explicit)) return explicit;

  const classList = element.classList;

  if (classList.contains('danger') || classList.contains('file-delete-btn')) {
    return 'danger';
  }

  if (classList.contains('primary') || classList.contains('auth-primary-btn') || classList.contains('save')) {
    return 'primary';
  }

  if (
    classList.contains('icon-btn') ||
    classList.contains('icon-copy-btn') ||
    classList.contains('snippet-copy-btn') ||
    classList.contains('profile-close')
  ) {
    return 'icon';
  }

  if (classList.contains('profile-tab') || classList.contains('theme-option') || classList.contains('sound-toggle')) {
    return 'toggle';
  }

  const label = (element.getAttribute('aria-label') || element.getAttribute('title') || '').toLowerCase();
  if (label.includes('delete') || label.includes('remove') || label.includes("o'chir") || label.includes('ochir')) {
    return 'danger';
  }

  return 'secondary';
};

const playPreset = (preset: SoundPreset): void => {
  const ctx = getAudioContext();
  if (!ctx) return;

  if (ctx.state === 'suspended') {
    void ctx.resume();
  }

  const now = ctx.currentTime;

  const clickSource = ctx.createBufferSource();
  clickSource.buffer = getNoiseBuffer(ctx);

  const clickFilter = ctx.createBiquadFilter();
  clickFilter.type = 'highpass';
  clickFilter.frequency.setValueAtTime(preset.clickHighpass, now);
  clickFilter.Q.setValueAtTime(0.7, now);

  const clickGain = ctx.createGain();
  clickGain.gain.setValueAtTime(0.0001, now);
  clickGain.gain.exponentialRampToValueAtTime(preset.clickGain, now + 0.002);
  clickGain.gain.exponentialRampToValueAtTime(0.0001, now + preset.clickDurationMs / 1000);

  clickSource.connect(clickFilter);
  clickFilter.connect(clickGain);
  clickGain.connect(ctx.destination);

  const toneOsc = ctx.createOscillator();
  toneOsc.type = preset.toneWave;
  toneOsc.frequency.setValueAtTime(preset.toneFrequency, now);
  if (preset.toneFrequencyEnd && preset.toneFrequencyEnd !== preset.toneFrequency) {
    toneOsc.frequency.exponentialRampToValueAtTime(
      preset.toneFrequencyEnd,
      now + preset.toneDurationMs / 1000
    );
  }

  const toneGain = ctx.createGain();
  const toneStart = now + (preset.toneDelayMs ? preset.toneDelayMs / 1000 : 0);
  toneGain.gain.setValueAtTime(0.0001, toneStart);
  toneGain.gain.exponentialRampToValueAtTime(preset.toneGain, toneStart + 0.004);
  toneGain.gain.exponentialRampToValueAtTime(0.0001, toneStart + preset.toneDurationMs / 1000);

  toneOsc.connect(toneGain);
  toneGain.connect(ctx.destination);

  let endedCount = 0;
  const cleanup = () => {
    endedCount += 1;
    if (endedCount < 2) return;
    clickSource.disconnect();
    clickFilter.disconnect();
    clickGain.disconnect();
    toneOsc.disconnect();
    toneGain.disconnect();
  };

  clickSource.onended = cleanup;
  toneOsc.onended = cleanup;

  clickSource.start(now);
  clickSource.stop(now + preset.clickDurationMs / 1000);

  toneOsc.start(toneStart);
  toneOsc.stop(toneStart + preset.toneDurationMs / 1000);
};

export const playButtonSound = (type: ButtonSoundType): void => {
  playPreset(BUTTON_PRESETS[type]);
};

export const playUiSound = (type: UiSoundType): void => {
  if (!isSoundEnabled()) return;
  playPreset(UI_PRESETS[type]);
};
