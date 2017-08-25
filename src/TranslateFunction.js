window.jsl10n.translate = function (path) {
    var n = window.jsl10n;
    path.split("/").forEach(function (element) {
        node = typeof node === "undefined" || node === null ? node : node[element];
    }, this);

    return node;
};