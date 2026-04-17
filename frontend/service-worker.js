// SkillMatch — Service Worker (PWA)
const CACHE = 'skillmatch-v1';
const STATIC = [
  '/',
  '/index.html',
  '/app.html',
  '/css/styles.css',
  '/js/api.js',
  '/pages/perfil.html',
  '/pages/chat.html',
  '/pages/cv-editar.html',
  '/pages/historico.html',
];

self.addEventListener('install', e => {
  e.waitUntil(caches.open(CACHE).then(c => c.addAll(STATIC)));
  self.skipWaiting();
});

self.addEventListener('activate', e => {
  e.waitUntil(caches.keys().then(keys =>
    Promise.all(keys.filter(k => k !== CACHE).map(k => caches.delete(k)))
  ));
  self.clients.claim();
});

self.addEventListener('fetch', e => {
  // Requisições à API sempre vão para a rede
  if (e.request.url.includes('/api/')) return;

  e.respondWith(
    caches.match(e.request).then(cached => cached || fetch(e.request))
  );
});
