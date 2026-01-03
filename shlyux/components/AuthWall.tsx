import React from 'react';
import { AlertCircle, Loader2, LogIn, UserPlus } from 'lucide-react';
import { AuthUser } from '../utils/api';
import { usePresence } from '../utils/usePresence';

interface AuthWallProps {
  isOpen: boolean;
  user: AuthUser | null;
  authMode: 'login' | 'register';
  authForm: { name: string; email: string; password: string };
  authError: string | null;
  authLoading: boolean;
  onChangeAuthForm: (field: 'name' | 'email' | 'password', value: string) => void;
  onSubmitAuth: () => void;
  onToggleMode: () => void;
}

const AuthWall: React.FC<AuthWallProps> = ({
  isOpen,
  user,
  authMode,
  authForm,
  authError,
  authLoading,
  onChangeAuthForm,
  onSubmitAuth,
  onToggleMode,
}) => {
  const shouldOpen = isOpen && !user;
  const modalPresence = usePresence(shouldOpen, { exitDurationMs: 180 });

  if (!modalPresence.isMounted) return null;

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSubmitAuth();
  };

  return (
    <div
      className="fixed inset-0 z-40 flex items-center justify-center px-4 backdrop-blur-md auth-overlay ui-overlay"
      data-state={modalPresence.state}
    >
      <div
        className="w-full max-w-[420px] rounded-2xl p-7 space-y-6 auth-card ui-modal"
        data-state={modalPresence.state}
      >
        <div className="flex items-start gap-3">
          <div>
            <p className="text-xs font-medium tracking-wide uppercase auth-kicker">W12C Sheets</p>
            <h2 className="text-xl font-semibold leading-tight" style={{ color: 'var(--text-primary)' }}>
              {authMode === 'register' ? "Ro'yxatdan o'ting" : 'Kirish'}
            </h2>
            <p className="text-sm mt-1" style={{ color: 'var(--text-secondary)' }}>
              {authMode === 'register'
                ? "Yangi hisob yarating — bir daqiqada tayyor."
                : 'Hisobingizga kiring va ishni davom ettiring.'}
            </p>
          </div>
        </div>

        {authError && (
          <div className="flex items-center gap-2 text-sm rounded-xl px-3 py-2.5 auth-error">
            <AlertCircle size={16} />
            <span>{authError}</span>
          </div>
        )}

        <form className="space-y-4" onSubmit={handleSubmit}>
          {authMode === 'register' && (
            <div className="space-y-1.5">
              <label className="auth-label" htmlFor="auth-name">Ism</label>
              <input
                id="auth-name"
                type="text"
                value={authForm.name}
                onChange={(e) => onChangeAuthForm('name', e.target.value)}
                placeholder="Ismingiz"
                className="auth-input"
                autoComplete="name"
                autoFocus
              />
            </div>
          )}
          <div className="space-y-1.5">
            <label className="auth-label" htmlFor="auth-email">Email</label>
            <input
              id="auth-email"
              type="email"
              value={authForm.email}
              onChange={(e) => onChangeAuthForm('email', e.target.value)}
              placeholder="name@example.com"
              className="auth-input"
              autoComplete="email"
              inputMode="email"
              autoFocus={authMode !== 'register'}
            />
          </div>
          <div className="space-y-1.5">
            <label className="auth-label" htmlFor="auth-password">Parol</label>
            <input
              id="auth-password"
              type="password"
              value={authForm.password}
              onChange={(e) => onChangeAuthForm('password', e.target.value)}
              placeholder="••••••••"
              className="auth-input"
              autoComplete={authMode === 'register' ? 'new-password' : 'current-password'}
            />
          </div>
          <button
            type="submit"
            disabled={authLoading}
            className="w-full h-11 inline-flex items-center justify-center gap-2 rounded-xl text-sm font-semibold disabled:opacity-60 transition-[filter,opacity,transform,box-shadow] btn-lift auth-primary-btn"
          >
            {authLoading ? (
              <>
                <Loader2 size={16} className="animate-rotate" />
                {authMode === 'register' ? 'Yaratilmoqda...' : 'Kirilmoqda...'}
              </>
            ) : (
              <>
                {authMode === 'register' ? <UserPlus size={16} /> : <LogIn size={16} />}
                {authMode === 'register' ? "Ro'yxatdan o'tish" : 'Kirish'}
              </>
            )}
          </button>
        </form>

        <div className="text-sm text-center" style={{ color: 'var(--text-secondary)' }}>
          {authMode === 'register' ? 'Hisobingiz bormi?' : "Hisob yo'qmi?"}{' '}
          <button
            onClick={onToggleMode}
            className="font-semibold auth-link"
          >
            {authMode === 'register' ? "Kirishga o'tish" : "Ro'yxatdan o'tish"}
          </button>
        </div>
      </div>
    </div>
  );
};

export default AuthWall;
