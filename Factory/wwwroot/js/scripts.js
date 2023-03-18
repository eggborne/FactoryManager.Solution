window.addEventListener('load', () => {
  document.documentElement.style.setProperty('--actual-height', window.innerHeight + 'px')
  // document.getElementById('engineer-splash-list').classList.remove('obscured');
  // document.getElementById('machine-splash-list').classList.remove('obscured');
  document.querySelector('main').classList.remove('obscured');
  console.log("window load event fired from <script> in _Layout")
});
