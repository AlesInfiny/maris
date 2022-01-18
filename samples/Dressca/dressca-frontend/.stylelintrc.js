module.exports = {
  extends: [
    'stylelint-config-recommended-vue',
    'stylelint-config-recommended-scss',
  ],
  overrides: [
    {
      files: ['**/*.scss'],
      customSyntax: 'postcss-scss',
    },
    {
      files: ['**/*.vue'],
      customSyntax: 'postcss-html',
    },
  ],
};
