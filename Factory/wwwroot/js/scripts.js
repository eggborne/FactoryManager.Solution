window.addEventListener('load', () => {
  document.documentElement.style.setProperty('--actual-height', window.innerHeight + 'px')
  document.querySelector('main').classList.remove('obscured');
});
