window.appCulture = {
    get: () => window.localStorage.getItem('equipitCulture'),
    set: (value) => window.localStorage.setItem('equipitCulture', value)
};
