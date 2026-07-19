// @title About
// @layout base
// @permalink /about/

open System

let html =
    md """
Hi, I'm **Exyone** — the creator of [Zest SSG](https://github.com/zest-ssg/zest)
and the porter of the [hugo-theme-terminal](https://github.com/panr/hugo-theme-terminal)
to Zest.

This is my personal English blog. I primarily write in Chinese over at
[exyon.ee](https://exyon.ee), and this site serves as a place to translate,
adapt, and share some of those posts with an English-speaking audience.
It also doubles as a living showcase for **Zest** and the **Terminal theme**.

Expect infrequent updates — I only post here when I find the time to translate
or when I have something worth sharing in English.

### What you'll find here

- **English adaptations** of selected posts from my Chinese blog
- **Zest SSG** demonstrations and development notes
- **Terminal theme** features and customization guides
- Random technical notes and thoughts
"""

printfn "%s" html
