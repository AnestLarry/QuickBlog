# Terminal Minimalist Redesign

## Overview
Redesign QuickBlog templates based on Google Stitch "Terminal Minimalist" design system. Dark theme with green accent, JetBrains Mono + Inter typography, brutalist sharp corners, scanline effects.

## Design Sources
- `stitch_minimalist_geek_blog/_1/code.html` — Blog index layout
- `stitch_minimalist_geek_blog/_2/code.html` — Tags page layout
- `stitch_minimalist_geek_blog/golang/code.html` — Category page layout
- `stitch_minimalist_geek_blog/post/code.html` — Post detail page layout
- `stitch_minimalist_geek_blog/terminal_minimalist/DESIGN.md` — Design system spec

## Approach
- Keep Tailwind CDN (user choice)
- Remove all existing CSS files except `highlight-11.9.0.css`
- Extract scanline/blink/cursor custom CSS into inline `<style>` blocks
- Share tailwind.config across all templates via `<script>` tag

## Template Mapping

| Design | Template | Notes |
|---|---|---|
| `_1/code.html` | `index.html` | Two-column: sidebar (25%) + article list (75%) |
| `_1/code.html` | `archive.html` | Same layout as index, different data source |
| `golang/code.html` | `category.html` | Three-tab sidebar + numbered post list |
| `golang/code.html` | `tag.html` | Same layout as category |
| `_2/code.html` | `tags.html` | Stats sidebar + tag grid |
| `post/code.html` | `post.html` | Single-column centered, code blocks with traffic light dots |

## Shared Partials

| Partial | Content |
|---|---|
| `navigation.liquid` | Fixed top navbar: "ROOT@USER:~$" brand, nav links, terminal/settings icons |
| `sidebar.liquid` | Left sidebar with CATEGORY/ARCHIVE tab switching, TAGS section, status card |
| `pagination.liquid` | Prev/Next pagination with page numbers, terminal-style `[PAGE 01 OF 08]` |
| `footer.liquid` (new) | Copyright bar with RSS/GITHUB/SOURCE links |

## Variables Used by Templates

From `Render.cs`:
- `{{ blog.Title }}`, `{{ blog.Categories }}`, `{{ blog.Tags }}`, `{{ blog.Archives }}`
- `{{ posts }}` — MarkdownInfoList
- `{{ post.Title }}`, `{{ post.Date }}`, `{{ post.Description }}`, `{{ post.ContentHTML }}`, `{{ post.Tags }}`, `{{ post.Categories }}`, `{{ post.URL }}`
- `{{ page.CurPage }}`, `{{ page.PageTotal }}`, `{{ page.PageRange }}`
- `{{ category }}` — current category/tag display name

## Files to Delete
- `static/css/global.css`
- `static/css/blog.css`
- `static/css/category.css`
- `static/css/post.css`
- `static/css/pagination.css`

## Files to Keep
- `static/css/highlight-11.9.0.css`
- `static/js/highlightjs-11.9.0.min.js`

## Design Tokens (Tailwind Config)

Color palette based on "Void" philosophy: deep near-black background (#131314) with Cyber Lime accent (#39FF14/#79FF5B). Surface layers use tonal stepping without shadows. All containers have 1px solid borders (outline-variant). Border-radius: 0px everywhere.

Typography: JetBrains Mono for UI/headlines, Inter for body text. Scale follows 4px vertical rhythm grid.
