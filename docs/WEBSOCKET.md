# WebSocket Protocol

## Connection
```javascript
const ws = new WebSocket("ws://localhost:4000/socket");
```

## Messages

### Cell Update
```json
{
  "type": "cell_update",
  "file_id": "uuid",
  "cell": "A1",
  "value": "Hello"
}
```

### Presence
```json
{
  "type": "presence",
  "user_id": 123,
  "cursor": "B5"
}
```

## Events
- `connected`
- `disconnected`
- `error`
- `message`
