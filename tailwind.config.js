/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./Pages/**/*.cshtml",
    "./Views/**/*.cshtml", 
    "./wwwroot/js/**/*.js"
  ],
  theme: {
    extend: {
      colors: {
        'todo-primary': '#3B82F6',
        'todo-secondary': '#10B981',
        'todo-accent': '#F59E0B'
      }
    },
  },
  plugins: [],
}