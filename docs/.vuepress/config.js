module.exports = {
  base: '/coolstore-microservices/',
  title: 'Coolstore Microservices',
  description: 'Coolstore Microservices',
  themeConfig: {
    displayAllHeaders: true,
    repo: 'vietnam-devs/coolstore-microservices',
    docsDir: 'docs',
    editLinks: false,
    editLinkText: 'Help us improve this page!',
    nav: [{ text: 'Home', link: '/' }],
    sidebar: ['/', '/model-microservices/', '/design-microservices/', '/development/']
  },
  head: [['link', { rel: 'icon', href: '/favicon.png' }]]
}
