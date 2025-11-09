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
        'todo-accent': '#F59E0B',
        // Primary Colors
        "deep-navy": "#1E293B",
        "dark-slate": "#0F172A",
        "electric-blue": "#3B82F6",
        "vibrant-teal": "#06D6A0",

        // Secondary
        "royal-blue": "#2563EB",
        "emerald": "#10B981",
        "amber": "#F59E0B",
        "soft-purple": "#8B5CF6",

        // Neutral Shades
        "dark-900": "#0A0F1C",
        "dark-800": "#1E293B",
        "dark-700": "#334155",
        "dark-600": "#475569",
        "dark-500": "#64748B",
        "dark-400": "#94A3B8",
        "dark-300": "#CBD5E1",
        "dark-200": "#E2E8F0",

        // Status
        "high-priority": "#EF4444",
        "medium-priority": "#F59E0B",
        "low-priority": "#10B981",
      }
    },
  },
  plugins: [],
}