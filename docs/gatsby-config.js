const settings = {
  config: `${__dirname}/site/sitemap.yml`,
  resources: `${__dirname}/site/docs`
}

module.exports = {
  pathPrefix: '/coolstore-microservices.github.io',
  siteMetadata: {
    title: 'coolstore-microservices',
    description:
      'The coolstore-microservices application builds for .NET ecosystem',
    githubUrl: 'https://github.com/vietnam-devs/coolstore-microservices',
    githubEditUrl:
      'https://github.com/vietnam-devs/coolstore-microservices/edit/master/docs/site',
    keywords: 'coolstore, microservices'
  },
  plugins: [
    {
      // local plugin, /plugins/docs
      resolve: 'docs',
      options: {
        config: settings.config
      }
    },
    'gatsby-plugin-react-helmet',
    'gatsby-transformer-yaml',
    {
      resolve: 'gatsby-source-filesystem',
      options: {
        name: 'content-pages',
        path: settings.resources
      }
    },
    {
      resolve: 'gatsby-transformer-remark',
      options: {
        plugins: [
          'gatsby-remark-prismjs',
          {
            resolve: 'gatsby-remark-images',
            options: {
              maxWidth: 600
            }
          },
          'gatsby-remark-autolink-headers'
        ]
      }
    }
  ]
}
