---
# Documentation: https://sourcethemes.com/academic/docs/managing-content/

title: "Game Design Document"
subtitle: "The design document for the Stealth AI project."
summary: "A document detailing the design of the Stealth AI project."
authors: ["Bart de Bever"]
tags: ["design"]
categories: ["design"]
date: 2019-09-10T11:02:15+02:00
lastmod: 2019-09-10T11:02:15+02:00
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

# Game Design Document

This document focusses on both the design of the toolkit I am working on and the
game that will be build around it. This game has the purpose of verifying the goals
set for the toolkit.

- [Game Design Document](#game-design-document)
  - [Game Analysis](#game-analysis)
  - [Game Design](#game-design)
    - [Target Audience](#target-audience)
    - [Goals of UX Design](#goals-of-ux-design)
    - [Story](#story)
    - [Theme](#theme)
    - [Mood](#mood)
    - [Boundaries & Settings](#boundaries--settings)
    - [Game World](#game-world)
    - [Characters](#characters)

## Game Analysis

To make sure the game and toolkit is design with stealth aspects in mind, I've
done multiple analysis on stealth games and their mechanics.

The first analysis was done on guard AI and predictability. Within this, patterns
were discovered about movement and movement indications on guards. These patterns
were confirmed by making a paper prototype and user testing this prototype. This
had an extra bonus about having a little analysis on the level design of these
games. People talked about how they were influenced by the level design and why
they thought some people went one way instead of the other.

## Game Design

This section explains the design around the game that was done.

### Target Audience

The target audience for this project is people who enjoy stealth games where
you are asked to recognize patterns and understand the behaviour of AI.

### Goals of UX Design

My user is meant to be challenged by the guards and the difficulty of the AI.
The user must have a feeling for learning how the guard moves and what the rules
are that they behave in.

The user should feel smart and accomplished when they overcome the challenges
imposed by the environment, guard and the mechanics.

### Story

You are a thief invading a jewellery for doing a heist.
You've gotten your loot and need to go outside now but accidentally tripped an alarm.
Guards have come and are patrolling the area. It is now up to you to evade
these guards and bring the treasure back home.

### Theme

The theme of this project is solo focussed on the stealth aspect.
The player is meant to avoid or deceive the guards by all means possible.
This can either be done by physically getting past the guards and evading them in
that way.
Or by using the special abilities that the players has to deceive the guards.

### Mood

The entire game is set during night time and, combined with the flashlights that
the guards have, gives a very mysterious and dangers mood.
The player should feel like every move could matter and a mistake shouldn't feel
like the player got cheated by a guard.

The primary emotions the player should feel is stress and satisfaction.

### Boundaries & Settings

The boundaries within the game lies in the actions that the player can perform to
influence the given situation.

- The player can move within the given room.
- The player is able to be caught by a guard.
- The player is unable to kill or disable guards permanently.
- The player is unable to leave the room without going through the provided exit.
- The player can distract guards to make a path for him/herself

### Game World

The story full plays within the jewellery store.
It might be further explored if there could be more scenery added.
The entire world is unaware of your existence let alone powers.
Things move as normal and life is pleasant for the inhabitants.

### Characters

You're a thief with special powers and are using your powers to steal as much as possible.
The guards are also there but have no special property to them.
There will be multiple types of guards with special characteristics.
