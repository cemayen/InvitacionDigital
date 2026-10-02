/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./Pages/**/*.{cshtml,razor}", "./Views/**/*.{cshtml,razor}", "./wwwroot/js/**/*.js"],
    theme: {
        extend: {
            container: {
                /* padding: {
                    DEFAULT: '1rem',
                    sm: '1rem',
                    md: '1rem,',
                    lg: '1rem',
                    xl: '0',
                    '2xl': '0',
                }, */
                screens: {
                    sm: "100%",
                    md: "100%",
                    lg: "980px",
                    xl: "980px",
                    "2xl": "980px",
                },
            },
            lineHeight: {
                100: "100%",
                120: "120%",
                125: "125%",
                150: "150%",
                175: "175%",
                200: "200%",
            },
            screens: {
                'xxxs': "345px",
                // => @media (min-width: 345px) { ... }
                'xxs': '390px',
                // => @media (min-width: 390px) { ... }
                'xs': '520px',
                // => @media (min-width: 520px) { ... }
                'sm': '640px',
                // => @media (min-width: 640px) { ... }
                'md': '768px',
                // => @media (min-width: 768px) { ... }
                'lg': '1024px',
                // => @media (min-width: 1024px) { ... }
                'xl': '1280px',
                // => @media (min-width: 1280px) { ... }
                '2xl': '1536px',
                // => @media (min-width: 1536px) { ... }
            },
            fontSize: {
                'xxs': '0.6rem',
                'xs': '0.75rem',    // 12px
                'sm': '0.875rem',   // 14px
                'base': '1rem',     // 16px
                'lg': '1.125rem',   // 18px
                'xl': '1.25rem',    // 20px
                '2xl': '1.5rem',    // 24px
                '3xl': '1.875rem',  // 30px
                '4xl': '2.25rem',   // 36px
                '5xl': '3rem',      // 48px
                '6xl': '3.75rem',   // 60px
                '7xl': '4.5rem',    // 72px
                '8xl': '6rem',      // 96px
                '9xl': '8rem',
            },
            colors: {
                shadow: {
                    500: "#00000030",
                },
                primary: {
                    500: "#D8B690",
                },
                secondary: {
                    500: "#92A68C"
                },
                durazno: "#FACDAE",
                arena: "#D8B690",
                salvia: "#92A68C",
                marfil:{
                    1: "#F5F3E2",
                    2: "#FDFCF8",
                },
                gris:{
                    100: "#DDD9CF",
                    500:"#5C5C5C",
                }
            }
        },
    },
    safelist: [
        "rotate-180",
        "opacity-0",
        "opacity-100",
        "fixed",
        "bottom-0",
    ],
    plugins: [],
}

