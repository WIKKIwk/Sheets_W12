/**
 * File Management API Examples
 */

// Get all files
async function getFiles(token) {
  const response = await fetch('http://localhost:8001/api/files', {
    headers: { 'Authorization': `Bearer ${token}` }
  });
  return response.json();
}

// Create new file
async function createFile(token, name) {
  const response = await fetch('http://localhost:8001/api/files', {
    method: 'POST',
    headers: {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({ name })
  });
  return response.json();
}

// Delete file
async function deleteFile(token, fileId) {
  await fetch(`http://localhost:8001/api/files/${fileId}`, {
    method: 'DELETE',
    headers: { 'Authorization': `Bearer ${token}` }
  });
}
