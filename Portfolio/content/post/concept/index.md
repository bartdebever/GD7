---
# Documentation: https://sourcethemes.com/academic/docs/managing-content/

title: "Concept"
subtitle: ""
summary: "A decently high level overview of the stealth AI system to be used."
authors: ["Bart de Bever"]
tags: []
categories: ["concept", "design", "analysis"]
date: 2019-09-11T08:25:08+02:00
lastmod: 2019-09-11T08:25:08+02:00
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

# System based stealth assets

This document will attempt to explain how the project will be implemented
within the context of Unity.
A minimal amount of knowledge of the Unity components is needed and no code
will be shown during this example.

## Included and excluded systems

To make sure this project is doable within the given timeframe, it is time to look
at what systems not to include within the system.

To start off, lets focus on the player, the first mechanic a developer would need
to implement is the camera and movement system of the player character.
This is something I will intend to leave at the developers implementation as this
varies a lot on a game by game basis.

The main focus of the project is on the guard AI and developing tools around it.
How would I implement an interface for being searching, being alerted, movement, etc?
How would you make these components and make them expandable for the developer without
them having to even open your source code?
How could I make the game play itself?

## Research and design possibilities

There are a ton of research possibilities available within this concept.
On the first side there is a possibility to look at things from the player side:

- How does a guard communicate where he is going?
- What does the guard think about the current situation?
- What can I get away with at this stage of alert?

There are still a ton of questions like this that can be asked and possibly answered.
The biggest focus I want to have here is on the movement of the guards.
How can a game developer make sure that the player understand where the guard is
going even with just getting a second of sight on the guard?

### Possible research questions

In this section a few research questions will be addressed that could be asked
and lead to interesting results. These questions are noted with the field that
they exist in within the *dot research framework*.

![Dot research framework image](attachments/DOT-Framework.jpg)

- How have existing games implemented the movement of guards using visuals? *(library)*
- How does the environment and the placement of the guards influence the thoughts
that the player has about the guard? *(lab)*
- Do stationary guards seem smarter to users than moving guards? *(lab)*
- How many movement patterns can a player remember at the same time? *(lab)*
- What are the most common complaints users have about guards in stealth games? *(library/field)*

### Design possibilities

In this section it will be discussed what could be designed ahead of developing this
project.

As discussed with both Jack (the teacher) and Bas (a student) the guards will
most likely use a state machine to perform actions under different conditions
and states of alert. There are multiple products that already do this for the developer
which could be used.
I would like to at least understand, design and implement the state machine myself
as a learning experience, even if this is not used afterwards.

The following image is an example state machine taken from google.

![Simple state machine example](attachments/statemachine.jpg)

Furthermore it will be important to work modularly and component based within
Unity itself. Developers should be able to expand the AI and systems themselves
and should not be limited to a type of movement because how I programmed my system.
