// @title Getting Started with Zest &amp; the Terminal Theme
// @date 2026-07-10
// @tags zest, terminal, ssg, demo
// @description A bundled showcase — learn how Zest SSG works, what the Terminal theme offers, and how to customize it.
// @layout single

open System

let html =
    divC "post-body" [
        md """
This article is a **bundled demo** that ships with the Zest SSG Terminal theme.
It combines several showcase posts into one walkthrough so you can quickly
understand what Zest and the Terminal theme can do out of the box.

---

## The Terminal Theme

The **Terminal** theme is a classic minimal blog theme. This port keeps the
same look and feel while running entirely on [Zest](https://zest.dev).

### What you get

- A single-column, responsive layout
- Light and dark color schemes
- Per-post tags and an archive
- An RSS feed at `/rss.xml` and a sitemap at `/sitemap.xml`

### A taste of code

```fsharp
// Define a page in Zest's F# DSL
page "hello" {
    title "Hello, Zest"
    content "<p>Generated with F#.</p>"
}
```

That is all it takes to start publishing.

---

## Building a Static Site with Zest

Zest reads your `content/` folder, renders Markdown to HTML, and wraps each
page with a layout. Templates are written in **Nunjucks** (`.njk`) and have
access to `site`, `page`, `pages`, and `tags`.

A minimal layout looks like this:

```njk
<!-- @layout base -->
<article class="post">
  <h1>{{ page.title }}</h1>
  <div class="post-content">{{ content | safe }}</div>
</article>
```

Because everything is pre-rendered, the result is a folder of plain `.html`
files you can host anywhere.

---

## Customizing the Theme

Most behavior is controlled through two small data files.

### Theme parameters

`_data/params.toml` holds values such as the subtitle, date format, and the
logo text:

```toml
subtitle = "A minimal, fast, and responsive theme for Zest."
dateFormat = "MMM d, yyyy"
showReadTime = true

[logo]
logoText = "Terminal"
```

### Navigation

`_data/nav.toml` drives the top menu:

```toml
[[items]]
label = "Home"
url = "/"
weight = 1
```

Edit these files, rebuild, and the site updates — no template changes needed.

---

## F# DSL Features

Beyond Markdown and Nunjucks, Zest pages can be written entirely in F# using
the Zest DSL. This gives you type safety, code reuse, and access to the full
.NET ecosystem at build time.

The following examples are from the bundled DSL demo — every feature shown
here is generated at build time by the F# DSL itself.

### Inline JavaScript

The `js` helper mirrors `md`: a triple-quoted string wrapped in `<script>` tags
in the final HTML, with automatic dedent so the body follows F# indentation.

```fsharp
divC "demo" [
    buttonC "btn" [ text "Click me" ]
    js "alert('Hello from F#!')"
]
```

### JSON data injection

`jsonBlock` serialises F# data to JSON and injects it as `window.NAME`.
No `sprintf` string concatenation, no XSS risk — `System.Text.Json` handles
all escaping, and `jsSafe` neutralises `</script>`-breaking sequences.

```fsharp
jsonBlock "__POST_DATA__" {|
    title = "Zest DSL Playground"
    tags = [|"fsharp"; "dsl"; "features"|]
    wordCount = 250
    published = DateTime.UtcNow.ToString("o")
|}
```

### Semantic components

```fsharp
breadcrumb [("Home", "/"); ("Posts", "/posts/"); ("Demo", "#")]

tagBadges "/tags/" ["fsharp"; "dsl"; "zcss"; "ssg"]

progressBar 80 "Build progress"
```

### Syntax sugar helpers

```fsharp
pluralize 3 "feature"        // → "3 features"
titleize "my-blog-post"      // → "My Blog Post"
capitalize "hello world"     // → "Hello world"
truncate_str 20 "A longer string that gets cut off"
```

### Inline Markdown with dedent

`mdDedent` strips common indentation before rendering, so you can keep
F# source formatting without breaking Markdown headings:

```fsharp
mdDedent "
    ### Why dedent matters

    Without dedent, an indented heading isn't recognised as a
    heading at all. `mdDedent` fixes this.

    - Bullet lists work
    - **Bold** and *italic* work
"
```

---

Ready to go further? Head to the
[deployment guide](/posts/deploy-to-cloudflare-pages/) to put your site online.
"""
    ]

printfn "%s" html
