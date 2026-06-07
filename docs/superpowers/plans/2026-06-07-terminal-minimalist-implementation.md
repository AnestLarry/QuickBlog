# Terminal Minimalist Redesign — Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task.

**Goal:** Replace all QuickBlog templates and static CSS with the Terminal Minimalist design from Google Stitch.

**Architecture:** Each page template (index, post, category, tag, tags, archive) inherits shared Tailwind config and partials (navigation, sidebar, pagination, footer). Design uses Tailwind CDN, Google Fonts (JetBrains Mono + Inter), and Material Symbols icons. All existing CSS files except highlight-11.9.0.css are deleted.

**Tech Stack:** QuickBlog (C# static gen), Fluid/Liquid templates, Tailwind CSS CDN v3, Google Fonts, Material Symbols.

---

### Task 1: Create shared config.liquid (Tailwind config + custom styles)

**Files:**
- Create: `templates/config.liquid`

- [ ] **Step 1: Write config.liquid**

```html
<script id="tailwind-config">
  tailwind.config = {
    darkMode: "class",
    theme: {
      extend: {
        colors: {
          "primary-fixed": "#79ff5b",
          "on-primary": "#053900",
          "primary-container": "#39ff14",
          "surface": "#131314",
          "surface-dim": "#131314",
          "surface-bright": "#3a393a",
          "surface-container-lowest": "#0e0e0f",
          "surface-container-low": "#1c1b1c",
          "surface-container": "#201f20",
          "surface-container-high": "#2a2a2b",
          "surface-container-highest": "#353436",
          "on-surface": "#e5e2e3",
          "on-surface-variant": "#baccb0",
          "background": "#131314",
          "on-background": "#e5e2e3",
          "outline": "#85967c",
          "outline-variant": "#3c4b35",
          "surface-variant": "#353436",
          "on-primary-container": "#107100",
          "on-primary-fixed": "#022100",
          "on-primary-fixed-variant": "#095300",
          "primary-fixed-dim": "#2ae500",
          "error": "#ffb4ab",
          "on-error": "#690005",
          "error-container": "#93000a",
          "on-error-container": "#ffdad6",
          "secondary": "#c8c6c7",
          "on-secondary": "#303031",
          "secondary-container": "#49494a",
          "on-secondary-container": "#bab8b9",
          "on-secondary-fixed": "#1b1b1c",
          "on-secondary-fixed-variant": "#474647",
          "tertiary": "#fbf9fb",
          "on-tertiary": "#303032",
          "tertiary-container": "#dfdcdf",
          "on-tertiary-container": "#616163",
          "tertiary-fixed": "#e4e2e4",
          "tertiary-fixed-dim": "#c8c6c8",
          "on-tertiary-fixed": "#1b1b1d",
          "on-tertiary-fixed-variant": "#474649",
          "secondary-fixed": "#e5e2e3",
          "secondary-fixed-dim": "#c8c6c7",
          "inverse-primary": "#106e00",
          "inverse-surface": "#e5e2e3",
          "inverse-on-surface": "#313031",
          "surface-tint": "#2ae500",
          "primary": "#efffe3"
        },
        spacing: {
          "unit": "4px",
          "gutter": "24px",
          "margin-desktop": "64px",
          "margin-mobile": "16px",
          "max-width": "1100px"
        },
        fontFamily: {
          "label-md": ["JetBrains Mono"],
          "headline-md": ["JetBrains Mono"],
          "code-sm": ["JetBrains Mono"],
          "headline-lg": ["JetBrains Mono"],
          "headline-lg-mobile": ["JetBrains Mono"],
          "body-lg": ["Inter"],
          "body-md": ["Inter"]
        },
        fontSize: {
          "label-md": ["14px", { lineHeight: "1.0", fontWeight: "500" }],
          "headline-md": ["24px", { lineHeight: "1.3", fontWeight: "600" }],
          "code-sm": ["13px", { lineHeight: "1.5", fontWeight: "400" }],
          "headline-lg": ["40px", { lineHeight: "1.1", letterSpacing: "-0.02em", fontWeight: "700" }],
          "headline-lg-mobile": ["28px", { lineHeight: "1.2", fontWeight: "700" }],
          "body-lg": ["18px", { lineHeight: "1.7", fontWeight: "400" }],
          "body-md": ["16px", { lineHeight: "1.6", fontWeight: "400" }]
        }
      }
    }
  }
</script>

<style>
  body {
    background-color: #131314;
    background-image: radial-gradient(circle, rgba(121, 255, 91, 0.05) 1px, transparent 1px);
    background-size: 32px 32px;
    scroll-behavior: smooth;
  }
  .scanline {
    width: 100%;
    height: 2px;
    background: rgba(121, 255, 91, 0.08);
    position: fixed;
    top: 0;
    z-index: 9999;
    pointer-events: none;
    animation: scanline 8s linear infinite;
  }
  @keyframes scanline {
    0% { transform: translateY(0); }
    100% { transform: translateY(100vh); }
  }
  .cursor-blink {
    animation: blink 1s step-end infinite;
  }
  @keyframes blink {
    50% { opacity: 0; }
  }
  ::-webkit-scrollbar { width: 4px; }
  ::-webkit-scrollbar-track { background: #131314; }
  ::-webkit-scrollbar-thumb { background: #3c4b35; }
  ::-webkit-scrollbar-thumb:hover { background: #79ff5b; }
  .terminal-cursor::after {
    content: "_";
    animation: blink 1s step-end infinite;
    color: #39ff14;
  }
  .code-block-header::before {
    content: "● ● ●";
    letter-spacing: 4px;
    color: #49494a;
    font-size: 10px;
    margin-right: 12px;
  }
  .material-symbols-outlined {
    font-variation-settings: "FILL" 0, "wght" 400, "GRAD" 0, "opsz" 24;
    vertical-align: middle;
  }
</style>
```

---

### Task 2: Create navigation.liquid

**Files:**
- Modify: `templates/navigation.liquid`

- [ ] **Step 1: Write navigation.liquid**

```html
<header class="fixed top-0 w-full z-50 flex justify-between items-center px-margin-desktop py-4 bg-background border-b border-outline-variant">
  <div class="flex items-center gap-2">
    <span class="font-headline-md text-headline-md font-bold text-primary-fixed tracking-tighter">ROOT@USER:~$</span>
    <span class="w-2 h-6 bg-primary-fixed cursor-blink"></span>
  </div>
  <nav class="hidden md:flex gap-8">
    <a class="font-label-md text-label-md text-on-surface-variant hover:text-primary-fixed transition-colors duration-150" href="/">Home</a>
    <a class="font-label-md text-label-md text-on-surface-variant hover:text-primary-fixed transition-colors duration-150" href="#">Posts</a>
    <a class="font-label-md text-label-md text-on-surface-variant hover:text-primary-fixed transition-colors duration-150" href="#">Projects</a>
    <a class="font-label-md text-label-md text-on-surface-variant hover:text-primary-fixed transition-colors duration-150" href="#">About</a>
  </nav>
  <div class="flex items-center gap-4">
    <span class="material-symbols-outlined text-primary-fixed cursor-pointer">terminal</span>
    <span class="material-symbols-outlined text-on-surface-variant hover:text-primary-fixed cursor-pointer">settings</span>
  </div>
</header>
```

---

### Task 3: Create sidebar.liquid

**Files:**
- Modify: `templates/sidebar.liquid`

- [ ] **Step 1: Write sidebar.liquid**

```html
<aside class="space-y-6">
  <div class="border border-outline-variant bg-surface-container-low">
    <div class="flex border-b border-outline-variant">
      <button class="flex-1 py-2 text-label-md font-label-md text-primary-fixed bg-surface-container-high border-r border-outline-variant" id="cat-tab" onclick="switchSidebarTab('category')">CATEGORY</button>
      <button class="flex-1 py-2 text-label-md font-label-md text-on-surface-variant hover:bg-surface-container" id="arc-tab" onclick="switchSidebarTab('archive')">ARCHIVE</button>
    </div>
    <div class="p-2 max-h-[500px] overflow-y-auto" id="sidebar-content">
      <div class="space-y-1" id="category-list">
        {% for category in blog.Categories %}
        <a href="/categories/{{ category.Key | replace: '/', '-' }}/1.html" class="flex justify-between items-center group cursor-pointer p-1 hover:bg-primary-fixed hover:text-on-primary">
          <span class="font-code-sm text-code-sm">[{{ category.Key | split: '/' | last }}]</span>
          <span class="font-code-sm text-code-sm">({{ category.Value }})</span>
        </a>
        {% endfor %}
      </div>
      <div class="hidden space-y-1" id="archive-list">
        {% assign current_year = "" %}
        {% for a in blog.Archives %}
          {% assign year = a | split: '/' | first %}
          {% if year != current_year %}
            {% if current_year != "" %}
              </div>
            {% endif %}
            <div class="font-code-sm text-code-sm text-primary-fixed px-1 py-2">{{ year }}</div>
            {% assign current_year = year %}
          {% endif %}
          <a href="/{{ a }}/1.html" class="block p-1 hover:bg-primary-fixed hover:text-on-primary font-code-sm text-code-sm text-on-surface-variant">{{ a | split: '/' | last }}</a>
        {% endfor %}
        {% if current_year != "" %}
          </div>
        {% endif %}
        {% assign current_year = "" %}
      </div>
    </div>
  </div>

  <div class="space-y-4">
    <h3 class="font-label-md text-label-md text-on-surface-variant flex items-center gap-2">
      <span class="material-symbols-outlined text-code-sm">tag</span> TAGS
    </h3>
    <div class="flex flex-wrap gap-2">
      {% for tag in blog.Tags %}
        {% if tag.Key and tag.Key != "" %}
        <a href="/tag/{{ tag.Key }}/index.html" class="px-2 py-1 border border-outline-variant text-code-sm font-code-sm hover:border-primary-fixed hover:text-primary-fixed transition-colors">#{{ tag.Key }}</a>
        {% endif %}
      {% endfor %}
    </div>
  </div>

  <div class="p-4 border border-outline-variant bg-surface-container-lowest">
    <div class="font-code-sm text-code-sm text-on-surface-variant mb-2">Uptime: 1420 days</div>
    <div class="w-full bg-surface-container h-1 mb-4 overflow-hidden">
      <div class="bg-primary-fixed h-full" style="width: 75%"></div>
    </div>
    <div class="font-code-sm text-code-sm text-primary-fixed flex items-center gap-2">
      <span class="material-symbols-outlined text-[14px]">bolt</span> HIGH_VOLTAGE_CORE
    </div>
  </div>
</aside>

<script>
  function switchSidebarTab(tab) {
    const catBtn = document.getElementById('cat-tab');
    const arcBtn = document.getElementById('arc-tab');
    const catList = document.getElementById('category-list');
    const arcList = document.getElementById('archive-list');
    if (tab === 'category') {
      catBtn.classList.add('text-primary-fixed', 'bg-surface-container-high');
      catBtn.classList.remove('text-on-surface-variant');
      arcBtn.classList.remove('text-primary-fixed', 'bg-surface-container-high');
      arcBtn.classList.add('text-on-surface-variant');
      catList.classList.remove('hidden');
      arcList.classList.add('hidden');
    } else {
      arcBtn.classList.add('text-primary-fixed', 'bg-surface-container-high');
      arcBtn.classList.remove('text-on-surface-variant');
      catBtn.classList.remove('text-primary-fixed', 'bg-surface-container-high');
      catBtn.classList.add('text-on-surface-variant');
      arcList.classList.remove('hidden');
      catList.classList.add('hidden');
    }
  }
</script>
```

---

### Task 4: Create pagination.liquid

**Files:**
- Modify: `templates/pagination.liquid`

- [ ] **Step 1: Write pagination.liquid**

```html
<div class="flex justify-between items-center font-code-sm text-code-sm mt-4 pt-12 border-t border-outline-variant">
  {% if page.CurPage > 1 %}
  <a href="./{{ page.CurPage | minus: 1 }}.html" class="px-4 py-2 border border-outline-variant text-primary-fixed hover:bg-surface-container-low transition-colors">[ PREV ]</a>
  {% else %}
  <span class="px-4 py-2 border border-outline-variant text-outline opacity-40 cursor-not-allowed">[ PREV ]</span>
  {% endif %}
  <div class="flex gap-2">
    {% for p in page.PageRange %}
      {% if p == page.CurPage %}
      <span class="px-3 py-2 bg-primary-fixed text-on-primary font-code-sm text-code-sm">{{ p | prepend: '0' | slice: -2, 2 }}</span>
      {% else %}
      <a href="./{{ p }}.html" class="px-3 py-2 border border-outline-variant text-on-surface-variant hover:text-primary-fixed transition-colors">{{ p | prepend: '0' | slice: -2, 2 }}</a>
      {% endif %}
    {% endfor %}
  </div>
  {% if page.CurPage < page.PageTotal %}
  <a href="./{{ page.CurPage | plus: 1 }}.html" class="px-4 py-2 border border-outline-variant text-primary-fixed hover:bg-surface-container-low transition-colors">[ NEXT ]</a>
  {% else %}
  <span class="px-4 py-2 border border-outline-variant text-outline opacity-40 cursor-not-allowed">[ NEXT ]</span>
  {% endif %}
</div>
```

---

### Task 5: Create footer.liquid

**Files:**
- Create: `templates/footer.liquid`

- [ ] **Step 1: Write footer.liquid**

```html
<footer class="w-full py-gutter px-margin-desktop flex flex-col md:flex-row justify-between items-center gap-4 opacity-80 border-t border-outline-variant mt-12">
  <div class="flex items-center gap-4">
    <span class="font-label-md text-label-md text-primary-fixed">DEV_CORE@SYSTEM:~$</span>
    <span class="font-code-sm text-code-sm text-on-surface-variant">&copy; 2024 DEV_CORE [TERMINAL_MODE_ENABLED]</span>
  </div>
  <div class="flex gap-6">
    <a class="font-code-sm text-code-sm text-on-surface-variant hover:text-primary-fixed transition-colors duration-150" href="#">RSS</a>
    <a class="font-code-sm text-code-sm text-on-surface-variant hover:text-primary-fixed transition-colors duration-150" href="#">GITHUB</a>
    <a class="font-code-sm text-code-sm text-on-surface-variant hover:text-primary-fixed transition-colors duration-150" href="#">SOURCE</a>
  </div>
</footer>
```

---

### Task 6: Rewrite index.html

**Files:**
- Modify: `templates/index.html`

- [ ] **Step 1: Write index.html**

```html
<!DOCTYPE html>
<html class="dark" lang="zh-CN">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>{{blog.Title}}</title>
  <script src="https://cdn.tailwindcss.com?plugins=forms,container-queries"></script>
  <link href="https://fonts.googleapis.com/css2?family=JetBrains+Mono:wght@400;500;600;700&family=Inter:wght@400;500&display=swap" rel="stylesheet">
  <link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined:wght,FILL@100..700,0..1&display=swap" rel="stylesheet">
  <link rel="stylesheet" href="/static/css/highlight-11.9.0.css" />
  {% include "config" %}
</head>
<body class="bg-background text-on-background font-body-md overflow-x-hidden selection:bg-primary-fixed selection:text-on-primary">
<div class="scanline"></div>

{% include "navigation" %}

<main class="mt-20 min-h-screen max-w-max-width mx-auto flex flex-col md:flex-row border-x border-outline-variant">
  <aside class="w-full md:w-1/4 border-b md:border-b-0 md:border-r border-outline-variant p-4">
    {% include "sidebar" %}
  </aside>

  <div class="w-full md:w-3/4 p-gutter space-y-8">
    <div class="flex items-center gap-2 font-code-sm text-code-sm text-on-surface-variant opacity-60">
      <span>~/users/dev/blog/posts</span>
      <span class="material-symbols-outlined text-[12px]">chevron_right</span>
      <span class="text-primary-fixed">all_entries.sh</span>
    </div>

    <div class="space-y-12">
      {% for post in posts %}
      <article class="group relative">
        <div class="flex flex-col md:flex-row md:items-baseline gap-4 mb-2">
          <span class="font-code-sm text-code-sm text-on-surface-variant whitespace-nowrap opacity-60">{{ post.Date | date: "%Y-%m-%d" }}</span>
          <h2 class="font-headline-md text-headline-md text-primary-fixed hover:underline cursor-pointer transition-all">
            <a href="/{{post.URL}}">{{ post.Title }}</a>
          </h2>
        </div>
        <div class="pl-0 md:pl-28">
          <p class="font-body-md text-body-md text-on-surface-variant leading-relaxed max-w-2xl">{{ post.Description }}</p>
          <div class="mt-4 flex gap-4">
            <a href="/{{post.URL}}" class="font-label-md text-label-md text-primary-fixed flex items-center gap-1 hover:translate-x-1 transition-transform">
              [READ_MORE] <span class="material-symbols-outlined text-[16px]">arrow_right_alt</span>
            </a>
          </div>
        </div>
      </article>
      {% endfor %}
    </div>

    {% include "pagination" %}
  </div>
</main>

{% include "footer" %}
</body>
</html>
```

---

### Task 7: Rewrite archive.html

**Files:**
- Modify: `templates/archive.html`

- [ ] **Step 1: Write archive.html** (same layout as index)

```html
<!DOCTYPE html>
<html class="dark" lang="zh-CN">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>{{blog.Title}} - Archives</title>
  <script src="https://cdn.tailwindcss.com?plugins=forms,container-queries"></script>
  <link href="https://fonts.googleapis.com/css2?family=JetBrains+Mono:wght@400;500;600;700&family=Inter:wght@400;500&display=swap" rel="stylesheet">
  <link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined:wght,FILL@100..700,0..1&display=swap" rel="stylesheet">
  {% include "config" %}
</head>
<body class="bg-background text-on-background font-body-md overflow-x-hidden selection:bg-primary-fixed selection:text-on-primary">
<div class="scanline"></div>

{% include "navigation" %}

<main class="mt-20 min-h-screen max-w-max-width mx-auto flex flex-col md:flex-row border-x border-outline-variant">
  <aside class="w-full md:w-1/4 border-b md:border-b-0 md:border-r border-outline-variant p-4">
    {% include "sidebar" %}
  </aside>

  <div class="w-full md:w-3/4 p-gutter space-y-8">
    <div class="flex items-center gap-2 font-code-sm text-code-sm text-on-surface-variant opacity-60 mb-4">
      <span class="text-primary-fixed">~/users/dev/blog/archives</span>
    </div>

    <div class="space-y-12">
      {% for post in posts %}
      <article class="group relative">
        <div class="flex flex-col md:flex-row md:items-baseline gap-4 mb-2">
          <span class="font-code-sm text-code-sm text-on-surface-variant whitespace-nowrap opacity-60">{{ post.Date | date: "%Y-%m-%d" }}</span>
          <h2 class="font-headline-md text-headline-md text-primary-fixed hover:underline cursor-pointer transition-all">
            <a href="/{{post.URL}}">{{ post.Title }}</a>
          </h2>
        </div>
        <div class="pl-0 md:pl-28">
          <p class="font-body-md text-body-md text-on-surface-variant leading-relaxed max-w-2xl">{{ post.Description }}</p>
          <div class="mt-4 flex gap-4">
            <a href="/{{post.URL}}" class="font-label-md text-label-md text-primary-fixed flex items-center gap-1 hover:translate-x-1 transition-transform">
              [READ_MORE] <span class="material-symbols-outlined text-[16px]">arrow_right_alt</span>
            </a>
          </div>
        </div>
      </article>
      {% endfor %}
    </div>

    {% include "pagination" %}
  </div>
</main>

{% include "footer" %}
</body>
</html>
```

---

### Task 8: Rewrite category.html

**Files:**
- Modify: `templates/category.html`

- [ ] **Step 1: Write category.html**

```html
<!DOCTYPE html>
<html class="dark" lang="zh-CN">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Category: {{ category }} - {{ blog.Title }}</title>
  <script src="https://cdn.tailwindcss.com?plugins=forms,container-queries"></script>
  <link href="https://fonts.googleapis.com/css2?family=JetBrains+Mono:wght@400;500;600;700&family=Inter:wght@400;500&display=swap" rel="stylesheet">
  <link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined:wght,FILL@100..700,0..1&display=swap" rel="stylesheet">
  {% include "config" %}
</head>
<body class="bg-background text-on-background font-body-md antialiased selection:bg-primary-fixed selection:text-on-primary">
<div class="scanline"></div>

{% include "navigation" %}

<main class="pt-24 pb-12 px-margin-mobile md:px-margin-desktop max-w-max-width mx-auto flex flex-col md:flex-row gap-gutter">
  <aside class="w-full md:w-64 shrink-0 flex flex-col gap-6">
    <div class="border border-outline-variant bg-surface-container-lowest">
      <div class="flex border-b border-outline-variant">
        <button class="flex-1 py-2 font-code-sm text-code-sm text-primary-fixed border-r border-outline-variant bg-surface-container" id="cat-tab2" onclick="switchCatTab('categories')">CATEGORIES</button>
        <button class="flex-1 py-2 font-code-sm text-code-sm text-on-surface-variant border-r border-outline-variant hover:text-primary-fixed" id="arc-tab2" onclick="switchCatTab('archives')">ARCHIVES</button>
        <button class="flex-1 py-2 font-code-sm text-code-sm text-on-surface-variant hover:text-primary-fixed" id="tag-tab2" onclick="switchCatTab('tags')">TAGS</button>
      </div>
      <div id="sidebar-tab-content">
        <div class="p-4 flex flex-col gap-2" id="categories-list2">
          {% for cat in blog.Categories %}
          <a href="/categories/{{ cat.Key | replace: '/', '-' }}/1.html" class="flex justify-between items-center group">
            <span class="font-code-sm text-code-sm text-on-surface-variant group-hover:text-primary-fixed group-hover:underline">{{ cat.Key | split: '/' | last }}</span>
            <span class="font-code-sm text-code-sm text-outline-variant group-hover:text-primary-fixed">[{{ cat.Value | prepend: '0' | slice: -2, 2 }}]</span>
          </a>
          {% endfor %}
        </div>
        <div class="hidden p-4 flex flex-col gap-2" id="archives-list2">
          {% for a in blog.Archives %}
          <a href="/{{ a }}/1.html" class="font-code-sm text-code-sm text-on-surface-variant hover:text-primary-fixed hover:underline">{{ a }}</a>
          {% endfor %}
        </div>
        <div class="hidden p-4 flex flex-wrap gap-2" id="tags-list2">
          {% for tag in blog.Tags %}
          <a href="/tag/{{ tag.Key }}/index.html" class="px-2 py-1 border border-outline-variant text-code-sm font-code-sm text-on-surface-variant hover:border-primary-fixed hover:text-primary-fixed">{{ tag.Key }}</a>
          {% endfor %}
        </div>
      </div>
    </div>

    <div class="border border-outline-variant bg-surface-container-lowest p-4">
      <div class="flex items-center gap-3 mb-4">
        <div class="w-10 h-10 bg-primary-container flex items-center justify-center">
          <span class="material-symbols-outlined text-on-primary-container" style="font-variation-settings: 'FILL' 1;">person</span>
        </div>
        <div>
          <div class="font-label-md text-label-md text-primary-fixed">DEV_USER</div>
          <div class="text-[10px] font-code-sm text-outline uppercase tracking-widest">SysAdmin Level 4</div>
        </div>
      </div>
      <div class="space-y-1">
        <div class="flex justify-between text-[11px] font-code-sm">
          <span class="text-outline">CPU_LOAD</span>
          <span class="text-primary-fixed">24%</span>
        </div>
        <div class="w-full h-1 bg-surface-container overflow-hidden">
          <div class="h-full bg-primary-fixed w-[24%]"></div>
        </div>
      </div>
    </div>
  </aside>

  <section class="flex-1 flex flex-col gap-6">
    <div class="border border-outline-variant bg-surface-container-low p-4 relative overflow-hidden">
      <div class="absolute top-0 right-0 p-2 opacity-10">
        <span class="material-symbols-outlined text-6xl">terminal</span>
      </div>
      <div class="flex items-center gap-2 font-code-sm text-code-sm text-outline-variant mb-2">
        <span>PATH:</span>
        <span class="text-primary-fixed">~/posts/categories/{{ category }}</span>
      </div>
      <h1 class="font-headline-lg-mobile md:font-headline-lg text-headline-lg-mobile md:text-headline-lg text-primary-fixed uppercase flex items-center">
        Category: {{ category }}<span class="terminal-cursor"></span>
      </h1>
    </div>

    <div class="flex flex-col gap-px bg-outline-variant border border-outline-variant">
      {% for post in posts %}
      <article class="bg-background p-6 hover:bg-surface-container-low transition-colors group cursor-pointer">
        <a href="/{{ post.URL }}">
          <div class="flex flex-col md:flex-row md:items-center justify-between gap-2 mb-2">
            <div class="flex items-center gap-3">
              <span class="font-code-sm text-code-sm text-outline">{{ forloop.index | prepend: '00' | slice: -3, 3 }}</span>
              <h2 class="font-headline-md text-headline-md text-on-surface group-hover:text-primary-fixed transition-colors">{{ post.Title }}</h2>
            </div>
            <time class="font-code-sm text-code-sm text-outline-variant">{{ post.Date | date: "%Y-%m-%d" }}</time>
          </div>
          <p class="font-body-md text-on-surface-variant mb-4 max-w-2xl">{{ post.Description }}</p>
          <div class="flex flex-wrap gap-2">
            {% for tag in post.Tags %}
            <span class="px-2 py-0.5 border border-outline-variant font-code-sm text-[11px] text-outline uppercase">#{{ tag }}</span>
            {% endfor %}
          </div>
        </a>
      </article>
      {% endfor %}
    </div>

    {% include "pagination" %}
  </section>
</main>

{% include "footer" %}

<script>
  function switchCatTab(tab) {
    const tabs = ['categories', 'archives', 'tags'];
    const tabIds = ['cat-tab2', 'arc-tab2', 'tag-tab2'];
    const listIds = ['categories-list2', 'archives-list2', 'tags-list2'];
    tabs.forEach((t, i) => {
      const btn = document.getElementById(tabIds[i]);
      const list = document.getElementById(listIds[i]);
      if (t === tab) {
        btn.classList.add('text-primary-fixed', 'bg-surface-container');
        btn.classList.remove('text-on-surface-variant');
        list.classList.remove('hidden');
      } else {
        btn.classList.remove('text-primary-fixed', 'bg-surface-container');
        btn.classList.add('text-on-surface-variant');
        list.classList.add('hidden');
      }
    });
  }
</script>
</body>
</html>
```

---

### Task 9: Rewrite tag.html

**Files:**
- Modify: `templates/tag.html`

- [ ] **Step 1: Write tag.html** (same layout as category.html)

```html
<!DOCTYPE html>
<html class="dark" lang="zh-CN">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Tag: {{ category }} - {{ blog.Title }}</title>
  <script src="https://cdn.tailwindcss.com?plugins=forms,container-queries"></script>
  <link href="https://fonts.googleapis.com/css2?family=JetBrains+Mono:wght@400;500;600;700&family=Inter:wght@400;500&display=swap" rel="stylesheet">
  <link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined:wght,FILL@100..700,0..1&display=swap" rel="stylesheet">
  {% include "config" %}
</head>
<body class="bg-background text-on-background font-body-md antialiased selection:bg-primary-fixed selection:text-on-primary">
<div class="scanline"></div>

{% include "navigation" %}

<main class="pt-24 pb-12 px-margin-mobile md:px-margin-desktop max-w-max-width mx-auto flex flex-col md:flex-row gap-gutter">
  <aside class="w-full md:w-64 shrink-0 flex flex-col gap-6">
    <div class="border border-outline-variant bg-surface-container-lowest">
      <div class="flex border-b border-outline-variant">
        <button class="flex-1 py-2 font-code-sm text-code-sm text-primary-fixed border-r border-outline-variant bg-surface-container" id="cat-tab3" onclick="switchCatTab3('categories')">CATEGORIES</button>
        <button class="flex-1 py-2 font-code-sm text-code-sm text-on-surface-variant border-r border-outline-variant hover:text-primary-fixed" id="arc-tab3" onclick="switchCatTab3('archives')">ARCHIVES</button>
        <button class="flex-1 py-2 font-code-sm text-code-sm text-on-surface-variant hover:text-primary-fixed" id="tag-tab3" onclick="switchCatTab3('tags')">TAGS</button>
      </div>
      <div class="p-4 flex flex-col gap-2" id="categories-list3">
        {% for cat in blog.Categories %}
        <a href="/categories/{{ cat.Key | replace: '/', '-' }}/1.html" class="flex justify-between items-center group">
          <span class="font-code-sm text-code-sm text-on-surface-variant group-hover:text-primary-fixed group-hover:underline">{{ cat.Key | split: '/' | last }}</span>
          <span class="font-code-sm text-code-sm text-outline-variant group-hover:text-primary-fixed">[{{ cat.Value | prepend: '0' | slice: -2, 2 }}]</span>
        </a>
        {% endfor %}
      </div>
      <div class="hidden p-4 flex flex-col gap-2" id="archives-list3">
        {% for a in blog.Archives %}
        <a href="/{{ a }}/1.html" class="font-code-sm text-code-sm text-on-surface-variant hover:text-primary-fixed hover:underline">{{ a }}</a>
        {% endfor %}
      </div>
      <div class="hidden p-4 flex flex-wrap gap-2" id="tags-list3">
        {% for tag in blog.Tags %}
        <a href="/tag/{{ tag.Key }}/index.html" class="px-2 py-1 border border-outline-variant text-code-sm font-code-sm text-on-surface-variant hover:border-primary-fixed hover:text-primary-fixed">{{ tag.Key }}</a>
        {% endfor %}
      </div>
    </div>
  </aside>

  <section class="flex-1 flex flex-col gap-6">
    <div class="border border-outline-variant bg-surface-container-low p-4 relative overflow-hidden">
      <div class="absolute top-0 right-0 p-2 opacity-10">
        <span class="material-symbols-outlined text-6xl">terminal</span>
      </div>
      <div class="flex items-center gap-2 font-code-sm text-code-sm text-outline-variant mb-2">
        <span>PATH:</span>
        <span class="text-primary-fixed">~/posts/tags/{{ category }}</span>
      </div>
      <h1 class="font-headline-lg-mobile md:font-headline-lg text-headline-lg-mobile md:text-headline-lg text-primary-fixed uppercase flex items-center">
        Tag: {{ category }}<span class="terminal-cursor"></span>
      </h1>
    </div>

    <div class="flex flex-col gap-px bg-outline-variant border border-outline-variant">
      {% for post in posts %}
      <article class="bg-background p-6 hover:bg-surface-container-low transition-colors group cursor-pointer">
        <a href="/{{ post.URL }}">
          <div class="flex flex-col md:flex-row md:items-center justify-between gap-2 mb-2">
            <div class="flex items-center gap-3">
              <span class="font-code-sm text-code-sm text-outline">{{ forloop.index | prepend: '00' | slice: -3, 3 }}</span>
              <h2 class="font-headline-md text-headline-md text-on-surface group-hover:text-primary-fixed transition-colors">{{ post.Title }}</h2>
            </div>
            <time class="font-code-sm text-code-sm text-outline-variant">{{ post.Date | date: "%Y-%m-%d" }}</time>
          </div>
          <p class="font-body-md text-on-surface-variant mb-4 max-w-2xl">{{ post.Description }}</p>
          <div class="flex flex-wrap gap-2">
            {% for tag in post.Tags %}
            <span class="px-2 py-0.5 border border-outline-variant font-code-sm text-[11px] text-outline uppercase">#{{ tag }}</span>
            {% endfor %}
          </div>
        </a>
      </article>
      {% endfor %}
    </div>

    {% include "pagination" %}
  </section>
</main>

{% include "footer" %}

<script>
  function switchCatTab3(tab) {
    const tabs = ['categories', 'archives', 'tags'];
    const tabIds = ['cat-tab3', 'arc-tab3', 'tag-tab3'];
    const listIds = ['categories-list3', 'archives-list3', 'tags-list3'];
    tabs.forEach((t, i) => {
      const btn = document.getElementById(tabIds[i]);
      const list = document.getElementById(listIds[i]);
      if (t === tab) {
        btn.classList.add('text-primary-fixed', 'bg-surface-container');
        btn.classList.remove('text-on-surface-variant');
        list.classList.remove('hidden');
      } else {
        btn.classList.remove('text-primary-fixed', 'bg-surface-container');
        btn.classList.add('text-on-surface-variant');
        list.classList.add('hidden');
      }
    });
  }
</script>
</body>
</html>
```

---

### Task 10: Rewrite tags.html

**Files:**
- Modify: `templates/tags.html`

- [ ] **Step 1: Write tags.html**

```html
<!DOCTYPE html>
<html class="dark" lang="zh-CN">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Tags - {{ blog.Title }}</title>
  <script src="https://cdn.tailwindcss.com?plugins=forms,container-queries"></script>
  <link href="https://fonts.googleapis.com/css2?family=JetBrains+Mono:wght@400;500;600;700&family=Inter:wght@400;500;600&display=swap" rel="stylesheet">
  <link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined:wght,FILL@100..700,0..1&display=swap" rel="stylesheet">
  {% include "config" %}
</head>
<body class="bg-background text-on-background font-body-md antialiased selection:bg-primary-fixed selection:text-on-primary">
<div class="scanline"></div>

{% include "navigation" %}

<main class="min-h-screen pt-32 pb-24 px-margin-mobile md:px-margin-desktop max-w-max-width mx-auto">
  <section class="mb-12 border-l-2 border-primary-fixed pl-6">
    <div class="flex items-center gap-2 mb-2">
      <span class="text-primary-fixed font-code-sm text-code-sm">SYSTEM_EXPLORER v2.0.4</span>
    </div>
    <h1 class="font-headline-lg text-headline-lg-mobile md:text-headline-lg text-on-surface uppercase tracking-tight">
      Tags<span class="inline-block w-[1ch] border-r-4 border-primary-fixed cursor-blink ml-2">&nbsp;</span>
    </h1>
    <p class="font-body-md text-body-md text-on-surface-variant mt-4 max-w-2xl">
      Index of metadata classifiers. Filtered by frequency and alphabetical weight.
      Use these identifiers to traverse the documentation archives.
    </p>
  </section>

  <div class="grid grid-cols-1 md:grid-cols-12 gap-0 border border-outline-variant bg-surface-container-lowest">
    <aside class="md:col-span-3 border-r border-outline-variant p-gutter">
      <div class="mb-8">
        <h3 class="font-label-md text-label-md text-primary-fixed mb-4 uppercase">Statistics</h3>
        <ul class="space-y-4 font-code-sm text-code-sm">
          <li class="flex justify-between border-b border-outline-variant pb-2">
            <span class="text-on-surface-variant">TOTAL_TAGS</span>
            <span class="text-primary-fixed">{{ blog.Tags.size | prepend: '0' | slice: -2, 2 }}</span>
          </li>
          <li class="flex justify-between border-b border-outline-variant pb-2">
            <span class="text-on-surface-variant">TOTAL_POSTS</span>
            <span class="text-primary-fixed">{{ posts.size | prepend: '0' | slice: -2, 2 }}</span>
          </li>
          <li class="flex justify-between border-b border-outline-variant pb-2">
            <span class="text-on-surface-variant">ACTIVE_TAGS</span>
            <span class="text-primary-fixed">ONLINE</span>
          </li>
        </ul>
      </div>
      <div class="p-4 bg-surface-container-low border border-outline-variant">
        <p class="font-code-sm text-code-sm text-primary-fixed leading-relaxed">
          [!] SYSTEM MESSAGE: Select a tag to initiate a query of the local data lake.
        </p>
      </div>
    </aside>

    <div class="md:col-span-9 p-gutter">
      <div class="flex flex-wrap gap-4 items-start content-start">
        {% for tag in blog.Tags %}
        <a href="/tag/{{ tag.Key }}/index.html" class="group inline-flex items-center gap-2 px-6 py-3 border border-outline-variant hover:border-primary-fixed transition-all duration-200">
          <span class="font-label-md text-label-md text-on-surface-variant group-hover:text-primary-fixed uppercase">{{ tag.Key }}</span>
          <span class="font-code-sm text-code-sm text-outline opacity-40">({{ tag.Value }})</span>
        </a>
        {% endfor %}
      </div>
    </div>
  </div>
</main>

{% include "footer" %}
</body>
</html>
```

---

### Task 11: Rewrite post.html

**Files:**
- Modify: `templates/post.html`

- [ ] **Step 1: Write post.html**

```html
<!DOCTYPE html>
<html class="dark" lang="zh-CN">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>{{post.Title}} | {{blog.Title}}</title>
  <script src="https://cdn.tailwindcss.com?plugins=forms,container-queries"></script>
  <link href="https://fonts.googleapis.com/css2?family=JetBrains+Mono:wght@400;500;600;700&family=Inter:wght@400;500;600&display=swap" rel="stylesheet">
  <link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined:wght,FILL@100..700,0..1&display=swap" rel="stylesheet">
  <link rel="stylesheet" href="/static/css/highlight-11.9.0.css" />
  {% include "config" %}
</head>
<body class="bg-background text-on-background font-body-md antialiased selection:bg-primary-fixed selection:text-on-primary">
<div class="scanline"></div>

{% include "navigation" %}

<main class="max-w-3xl mx-auto px-margin-mobile md:px-0 py-16">
  <header class="mb-12">
    <div class="flex items-center gap-2 mb-4">
      <span class="text-primary-fixed font-label-md text-label-md">0x00_ENTRY</span>
      <div class="h-[1px] flex-grow bg-outline-variant"></div>
    </div>
    <h1 class="font-headline-lg text-headline-lg md:text-headline-lg text-on-surface mb-8 terminal-cursor leading-tight">
      {{post.Title}}
    </h1>
    <div class="border border-outline-variant bg-surface-container-lowest p-6 flex flex-col md:flex-row gap-6 md:items-center font-code-sm text-code-sm">
      <div class="flex items-center gap-3">
        <span class="text-primary-fixed opacity-60">DATE:</span>
        <span class="text-on-surface">{{ post.Date | date: "%Y-%m-%d" }}</span>
      </div>
      {% if post.Categories.size > 0 %}
      <div class="hidden md:block w-px h-4 bg-outline-variant"></div>
      <div class="flex items-center gap-3">
        <span class="text-primary-fixed opacity-60">CATEGORIES:</span>
        <div class="flex gap-2">
          {% for category in post.Categories %}
          <a href="/categories/{{ category | replace: '/', '-' }}/index.html" class="px-2 py-0.5 border border-primary-fixed/30 text-primary-fixed">[{{ category }}]</a>
          {% endfor %}
        </div>
      </div>
      {% endif %}
      {% if post.Tags.size > 0 %}
      <div class="hidden md:block w-px h-4 bg-outline-variant"></div>
      <div class="flex items-center gap-3">
        <span class="text-primary-fixed opacity-60">TAGS:</span>
        <div class="flex gap-2">
          {% for tag in post.Tags %}
          <a href="/tag/{{ tag }}/index.html" class="px-2 py-0.5 border border-primary-fixed/30 text-primary-fixed">[{{ tag }}]</a>
          {% endfor %}
        </div>
      </div>
      {% endif %}
      <div class="hidden md:block w-px h-4 bg-outline-variant"></div>
      <div class="flex items-center gap-3">
        <span class="text-primary-fixed opacity-60">STATUS:</span>
        <span class="text-on-primary-container">STABLE</span>
      </div>
    </div>
  </header>

  <article class="space-y-8 font-body-lg text-body-lg text-on-surface-variant leading-relaxed">
    {{post.ContentHTML}}
  </article>
</main>

{% include "footer" %}

<script src="/static/js/highlightjs-11.9.0.min.js"></script>
<script>hljs.highlightAll();</script>
</body>
</html>
```

---

### Task 12: Delete old CSS files

**Files:**
- Delete: `static/css/global.css`
- Delete: `static/css/blog.css`
- Delete: `static/css/category.css`
- Delete: `static/css/post.css`
- Delete: `static/css/pagination.css`

- [ ] **Step 1: Delete unused CSS files**

Run:
```powershell
Remove-Item -LiteralPath "static/css/global.css" -Force; if ($?) { Remove-Item -LiteralPath "static/css/blog.css" -Force }; if ($?) { Remove-Item -LiteralPath "static/css/category.css" -Force }; if ($?) { Remove-Item -LiteralPath "static/css/post.css" -Force }; if ($?) { Remove-Item -LiteralPath "static/css/pagination.css" -Force }
```

---

### Task 13: Build and verify

**Files:**
- Test: Build QuickBlog project and check output

- [ ] **Step 1: Build and run QuickBlog to generate static files**

Run:
```powershell
dotnet build -c Release; if ($?) { dotnet run --project . -- -o output }
```

Expected: Build succeeds, static files generated in `output/` directory.

- [ ] **Step 2: Spot-check output HTML files**

Run:
```powershell
Get-ChildItem -LiteralPath "output" -Recurse -Filter "*.html" | Select-Object -First 5
```

Verify files contain Tailwind classes and new design elements.

---

## Spec Coverage Check

| Spec Requirement | Task |
|---|---|
| Shared Tailwind config | Task 1 |
| Top navbar (ROOT@USER:~$) | Task 2 |
| Sidebar with tabs | Task 3 |
| Pagination with prev/next + page nums | Task 4 |
| Footer partial | Task 5 |
| Index page (two-column) | Task 6 |
| Archive page | Task 7 |
| Category page (three-tab sidebar) | Task 8 |
| Tag page | Task 9 |
| Tags listing (stats + grid) | Task 10 |
| Post detail (centered, code blocks) | Task 11 |
| Remove old CSS | Task 12 |
| Build verification | Task 13 |
