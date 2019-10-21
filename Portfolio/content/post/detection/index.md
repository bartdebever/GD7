---
# Documentation: https://sourcethemes.com/academic/docs/managing-content/

title: "Detection"
subtitle: "An analysis on detection from guards in games"
summary: ""
authors: []
tags: []
categories: ["analysis"]
date: 2019-10-04T09:12:39+02:00
lastmod: 2019-10-04T09:12:39+02:00
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

Now the movement system has been analysed, designed and implemented, lets look at
the next logical step in stealth gameplay: **Detection**.

With detection, I mean the way that guards or cameras can see you and be alerted.
How close can you get to a guard, how far away can he see and how wide is his viewing
angle.

## How close can the player get to a guard

While a bit of the easier question to answer, it could still proof difficult to
not let the player go out of control. This question may heavily depend on your game.
In `Hitman` or `Dishonored`, it may be required to get close to a guard to eliminate
then and take their disguise. Yet in games like `PAYDAY 2` or even stealth sections
in non-stealth games, it might be critical that the user does not get close to a
guard.

When the player should not get close to a guard it should be communicated clearly,
perhaps in a tutorial that shows the player another thief getting caught or a
simple help text can work as well. The most important thing is that the rules
are simple and communicated properly to the player. The alert system should be used
here where the guard could "hear" the players footsteps within a given area and
makes a remark about that. Do not let the guard turn around instantly, give the
player time to escape and do their thing.

Do not forget the rule of consistency here, make sure that all guards or NPCs that
can spot the player have the same radius that they will detect the player in.

## Types of vision games and/or Unity uses

The types of vision used is not strictly defined within game design.
Some games simply check if the playeer is within a given radius of the guard while
others have a strict vision cone.

The most popular form of vision is using ray casts to hit an object and detect it.
