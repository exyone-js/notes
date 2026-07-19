// _init.zest.fsx — pre-build hook.
//
// After the §1.2/1.3 engine fix, TOML arrays from _data/*.toml are preserved
// as native .NET arrays/dictionaries, so Nunjucks can iterate them directly.
// The _data/nav.toml file is auto-loaded as `site.nav.items` — no manual
// parsing or pre-rendered HTML needed. The header include now uses:
//
//   {% for item in site.nav.items | sort_by("weight") %}
//     <li><a href="{{ item.url }}">{{ item.label }}</a></li>
//   {% endfor %}
//
// This file remains for any additional global data or computation you need
// before the build runs.

// Example: expose build timestamp for cache-busting query strings.
addGlobal "build_time" (System.DateTime.UtcNow.ToString("yyyyMMddHHmmss"))

// Example: expose social links as a structured array (usable in Nunjucks).
addGlobal "socials" [|
    {| label = "GitHub";  url = "https://github.com/exyone-js"; icon = "github" |}
    {| label = "Twitter"; url = "https://twitter.com/exyone_js"; icon = "twitter" |}
|]
