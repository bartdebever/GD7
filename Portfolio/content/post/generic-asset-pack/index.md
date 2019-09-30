---
# Documentation: https://sourcethemes.com/academic/docs/managing-content/

title: "Generic Asset Pack"
subtitle: "What methods are available to create a reusable asset pack?"
summary: ""
authors: []
tags: []
categories: ["analysis", "design", "showroom"]
date: 2019-09-25T11:48:56+02:00
lastmod: 2019-09-25T11:48:56+02:00
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

Within this document there will be an analysis about how a generic asset pack can
be created to satisfy the design challenge set in the
[concept document]({{< relref "/post/concept/index.md" >}}).

This is going to be a more technical document due to being about development.
Code examples may be given for C# code.

## Pre-existing knowledge

Due to having made some NuGet packages and trying to split up my own personal
workload into different applications and projects, I've had some experience working
with making code generic. Within C# there exist a lot of language features that
help with making code generic. *Interfaces*, *abstract classes*, *Type parameters*,
all these tools help making and implementing generic code easier for developers.

Unity has a layer on top of this as well, the two main abstract classes within Unity
are *GameObject* and *MonoBehaviour*.

*GameObject* is the base class that every
object is extended from. A Cube, Sphere, Triangle or your entire model (container)
is a *GameObject*. Even things like the camera, lighting and non physical objects
that control systems are *GameObjects*.

*MonoBehaviour* is the class that most scripts inherit from. This classifies them
as having access to the Unity systems. A script has to always be place on a *GameObject*.
Other classes that are not *MonoBehaviours* do not have access to the Unity systems
and are not attachable to a *GameObject*.

## Patterns

Within software there are multiple patterns that a developer can follow to implement
features. For making generic and expandable classes there are multiple patterns in place.
This chapter will contain information about those patterns, their upsides and downsides.

### Event driven

Within C#, an user can set up *Delegates*. These are objects that takes zero
to many subscriptions and that can fire an *event* at a given time.
This allows for the code to be separated and not know anything about each other.
The class that fires the event may supply data to the event that will be passed
to each subscriber.

The following code snippit is an easy implementation of an event firing class.
This snippit is taken from [Unitycoder.com](https://i0.wp.com/unitycoder.com/blog/wp-content/uploads/2015/01/events_delegates.jpg?fit=680%2C400&ssl=1),
for the record, I do not recommend such an easy implementation and anything in
the created prototype will be more build out than this.

```cs
public class FireEvent
{
  public delegate void Clicked();
  public event Clicked WasClicked;

  public void TriggerEvent()
  {
    // If there are no subscriptions to the event it will be null.
    if (WasClicked == null)
    {
        // Nobody has subscribed to the event, there is no need to trigger it.
        return;
    }

    // Triggers the event and Invokes the methods that are subscribed.
    WasClicked();
  }
}
```

The upsides of this pattern is that Unity/C# has native support and allows for
an easy implementation. Unity and the community around Unity have made multiple
guides on how to use this pattern and it's also been taught before.

The downside of this pattern is the clutter that it can become. It's hard to figure
out when the event is triggered, in what order the methods will be called.
When an event trigger is in an asset pack's source code, it's also potentially hard
to find all the use cases of it within your application.

With this pattern it is also **impossible** for the subscribed methods to return a value.

### Callbacks

While callbacks aren't the most used thing in C#, it is a common practise in other
languages like Java. This pattern is very similar to the **Event pattern**, the
developer has the option to supply a method that will be executed at some point
in time. Callbacks in software are mostly used to perform actions after another
is done.

> When the user clicks the mouse button, perform this action.
>
> When the screen is drawn, perform this other action.

The following code example was taken from the [Callbacks wikipedia page](https://en.wikipedia.org/wiki/Callback_(computer_programming))

```cs
public class Class
{
    /*
    * The method that calls back to the caller. Takes an action (method) as parameter
    */
    public void Method(Action<string> callback)
    {
        /*
        * Calls back to method CallBackMet in Class1 with the message specified
        */
        callback("The message to send back");
    }
}
```

The advantage of the callback is the freedom that it brings.
Using generics, interfaces and other techniques, each method that's added to the
callback list could have its own inputs and outputs.

The disadvantage of callbacks is the complexity that it can create.
Callbacks in some languages are done as inline *lambda* statements.
These can be very badly readable and hard to trace back.

The code for calling each callback method also has to be written for the developer
and this can give more difficulties:

- What if there is an exception?
- What if one method takes too long?
- What if I don't have methods?
- How many methods should I call maximum?

### Unity Component System

Like the name applies, the Component system that Unity uses could be used to set up
communication between different components. The components come in as a normal C# class
would do and the sender has access to all public methods and fields that the
receiver has. This communication could possibly go two ways, the sender calls a method
on the receiver that has a return value and processes it further.

This style of communication is often seen in such Manager classes. They are a thing
that most Unity developers come back on when making systems that need to be given
a direction. This could actually be compared to a real life manager, having a view
over resources and dedicated them where needed.

> At this time I unfortunately don't have a clever picture or code snippit to share.

The upside is the build in support for this kind of system and that the developer
is already used to it because of it. Two way communication is also easy to apply.
Use of interfaces and abstract classes is possible.

The downside is that this has a dependency on Unity and MonoBehaviour. This isn't
a big deal as we are in Unity but with things like ECS on the rise, it may be a problem
later. On that note...

### Entity Component System

Unity is working towards a new performant data driven way of working on applications.
For short this is called ECS. The system classes are what the user could use to
implement things like movement based on the data we provide from the package very easily.
The issue is that this system is still in preview and it shows. Documentation
is rather limited and so is the knowledge base.

The following code is to rotate a cube using the ECS System class, this comes
from [the official Unity3D documentation](https://docs.unity3d.com/Packages/com.unity.entities@0.1/manual/entity_iteration_job.html).

```cs
public class RotationSpeedSystem : JobComponentSystem
{
   // Use the [BurstCompile] attribute to compile a job with Burst.
   [BurstCompile]
   struct RotationSpeedJob : IJobForEach<RotationQuaternion, RotationSpeed>
   {
       public float DeltaTime;
       // The [ReadOnly] attribute tells the job scheduler that this job will not write to rotSpeed
       public void Execute(ref RotationQuaternion rotationQuaternion, [ReadOnly] ref RotationSpeed rotSpeed)
       {
           // Rotate something about its up vector at the speed given by RotationSpeed.
           rotationQuaternion.Value = math.mul(math.normalize(rotationQuaternion.Value), quaternion.AxisAngle(math.up(), rotSpeed.RadiansPerSecond * DeltaTime));
       }
   }

// OnUpdate runs on the main thread.
// Any previously scheduled jobs reading/writing from Rotation or writing to RotationSpeed
// will automatically be included in the inputDependencies.
protected override JobHandle OnUpdate(JobHandle inputDependencies)
   {
       var job = new RotationSpeedJob()
       {
           DeltaTime = Time.deltaTime
       };
       return job.Schedule(this, inputDependencies);
   }
}
```

It would be really awesome to implement this but as discovered today with the workshop,
the system can be very complex to program and has a steep learning curve.
I wanted to include this for the sake of having it noted down.

## Available product analysis

To make sure that the previous mentioned products are actually being used,
there is going to be a look into the top 10 popular AI asset packs within the
Unity asset store and check out what pattern they follow

| Name                           | Pattern                 | Comment                                               | Proof                                                                               |       |
|--------------------------------|-------------------------|-------------------------------------------------------|-------------------------------------------------------------------------------------|-------|
| A* Pathfinding Project Pro     | Event based             | Calls the method callbacks but they are delgates      | https://arongranberg.com/astar/docs/custom_movement_script.html                     |       |
| Behavior Designer - Movement   | Event based             |                                                       | https://opsive.com/support/documentation/behavior-designer/behavior-tree-component/ |       |
| Dialogue System                | Unity Component System  |                                                       | http://www.pixelcrushers.com/dialogue_system/manual2x/html/quick_start.html         |       |
| Emerald AI                     | Unity Component System  | Uses events for expansions                            | http://www.pixelcrushers.com/dialogue_system/manual2x/html/quick_start.html         |       |
| Behavior Designer - Tactical   |                         | Skipped for being too same to a package.              |                                                                                     |       |
| Simple Tile Pathfinding        | Unity Component System? | Educated guess as there is no documentation provided. | https://assetstore.unity.com/packages/tools/ai/simple-tile-pathfinding-136493       |       |
| Fish Flock                     | Unity Component System  | As seen in the screenshot                             | https://assetstore.unity.com/packages/tools/ai/fish-flock-100717                    |       |
| Behavior Designer - Formations |                         | Skipped for being too same to a package.              |                                                                                     | being |
| FSM AI Template                | Unity Component System  |                                                       | https://www.invector.xyz/aidocumentation                                            |       |
| Aircraft AI system             |                         | No documentation                                      |                                                                                     |       |
| PolyNav - 2D Pathfinding       | Callback or Event based | Events are an alternative                             | https://drive.google.com/file/d/120M2csM2dA6uPVdg_ZIU0BSsPuJKVQJW/view              |       |

This turned out to not be 10 asset packs as the pack were starting to get no
documentation and barely any user reviews, unrepresentative.

From the results we can see that *Unity Component System* is used the most with
*Event Based* as a close second.

This means in my project I will use the *Unity Component System* and *Event Based*.
