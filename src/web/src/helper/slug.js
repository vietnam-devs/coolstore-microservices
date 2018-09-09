import slugify from 'slugify'

export const slug = str => slugify(str, { lower: true })