---
# Documentation: https://sourcethemes.com/academic/docs/managing-content/

title: "Quickloading"
subtitle: ""
summary: ""
authors: []
tags: []
categories: ["design", "uml"]
date: 2019-10-28T10:11:00+01:00
lastmod: 2019-10-28T10:11:00+01:00
featured: false
draft: false

# Featured image
# To use, add an image named `featured.jpg/png` to your page's folder.
# Focal points: Smart, Center, TopLeft, Top, TopRight, Left, Right, BottomLeft, Bottom, BottomRight.
image:
  caption: ""
  focal_point: ""
  preview_only: false

# Projects (optional).
#   Associate this post with one or more of your projects.
#   Simply enter your project's folder or file name without extension.
#   E.g. `projects = ["internal-project"]` references `content/project/deep-learning/index.md`.
#   Otherwise, set `projects = []`.
projects: ["stealth-ai"]
---

Within my analysis of [Dishonored 2]({{< ref "/post/dishonored2/index.md" >}}), I noted the importance of the quick save/load
system. It was essential in making the experience feel good and make mistakes feel
not as bad as it may seem.

Because of this, I've decided I want to implement the quick saving and loading within
the asset pack.

The following is an UML Design of the set in place system.

![UML Design Diagram](attachments/QuickLoading.png)
