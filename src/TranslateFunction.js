window.jsl10n.translate = function (path) {
    return path.split('/').reduce(function (o, x) {
        return typeof o === "undefined" || o === null ? o : o[x];
    }, window.jsl10n);
};