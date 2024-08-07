/** @type {import('tailwindcss').Config} */
import colors, { yellow } from "tailwindcss/colors";

export default {
  content: ['./index.html', './src/**/*.{vue,js,ts,jsx,tsx}'],
  theme: {
    // IntelliSenseによる補完を使いやすくするために、使用する色を絞ります。
    colors:{
      black: colors.black,
      white: colors.white,
      gray: colors.gray,
      red: colors.red,
      green: colors.green,
      orange: colors.orange
    },
    extend:{
      // プロジェクト独自の色を追加できます。
      colors:{
        'light-blue': {
          50:  '#f0f9ff',
          600: '#008bf2',
          800: '#0066be',
          1000: '#00428c',
        },
    },
  },
  plugins: [],
}
}
