# Chat Application

## How To Run
For running Nats message broker in your system you can use of this [docker compose](./deploys/compose/docker-compose.yaml) file.

For running our system we need to run our [Console Application](./src/Chat.Console) as client and [API](./src/Chat.Api) as a service. we can use bellow command in our shell to running our clients and api. Our api will up and running on this `http://localhost:8000` address.

``` cmd
.\scripts\backend-api.bat
.\scripts\console-ui.bat
```
we can create multiple instance of console application as our chat UI and we can chat between these separated console instances, we have to run `.\scripts\console-ui.bat` multiple to create multiple instance of chat application.

## Chat Architecture Diagram

![](./assets/diagram.png)

In above diagram I show how our components connect together for running this chat application. Here we have two console application or two instance of console application as our Chat UI, one NATS message broker and one API service for get and post some data.

The flow of our application is like this:

- Our first console application (instance 1) as Chat UI sends a message data to our API service.
- Our API endpoint will get posted message from the client and process and save this message in a in-memory storage (EF-Core In-memory) and then publish this message as a event to our NATS message broker. This message contain needed information for our subscribers for example `sender` , `receiver`, `message` and our subscribers can pick their needed information from theses events.

- Our second console application (instance 2) as Chat UI, that is subscribed to that event through our NATS message broker, receives this message from the broker and will show on the console.
## Structure of Project

I used a [mediator pattern](https://dotnetcoretutorials.com/2019/04/30/the-mediator-pattern-in-net-core-part-1-whats-a-mediator/) with using [MediatR](https://github.com/jbogard/MediatR) library in my controllers for a clean and [thin controller](https://codeopinion.com/thin-controllers-cqrs-mediatr/), also instead of using a `application service` class because after some times our controller will depends to different services and this breaks single responsibility principle.mediator to manage the delivery of messages to handlers. One of the advantages behind the [mediator pattern](https://lostechies.com/jimmybogard/2014/09/09/tackling-cross-cutting-concerns-with-a-mediator-pipeline/) is that it allows the application code to define a pipeline of activities for requests . For example in our controllers we create a command and send it to mediator and mediator will route our command to a specific command handler in application layer. 

To support [Single Responsibility Principle](https://en.wikipedia.org/wiki/Single_responsibility_principle) and [Don't Repeat Yourself principles](https://en.wikipedia.org/wiki/Don%27t_repeat_yourself), the implementation of cross-cutting concerns is done using the mediatr [pipeline behaviors](https://github.com/jbogard/MediatR/wiki/Behaviors) or creating a [mediatr decorators](https://lostechies.com/jimmybogard/2014/09/09/tackling-cross-cutting-concerns-with-a-mediator-pipeline/).

Also in this project I used [vertical slice architecture](https://jimmybogard.com/vertical-slice-architecture/) or [package by component](http://www.codingthearchitecture.com/2015/03/08/package_by_component_and_architecturally_aligned_testing.html) and also I used [feature folder structure](http://www.kamilgrzybek.com/design/feature-folders/) in this project.

Also here I used cqrs for decompose my features to very small parts that make our application

- maximize performance, scalability and simplicity.
- adding new feature to this mechanism is very easy without any breaking change in other part of our codes. New features only add code, we're not changing shared code and worrying about side effects.
- easy to maintain and any changes only affect on one command or query and avoid any breaking changes on other parts
- it gives us better separation of concerns and cross cutting concern (with help of mediatr behavior pipelines) in our code instead of a big service class for doing a lot of things.  

I treat each request as a distinct use case or slice, encapsulating and grouping all concerns from front-end to back.
When adding or changing a feature in an application in n-tire architecture, we are typically touching many different "layers" in an application. we are changing the user interface, adding fields to models, modifying validation, and so on. Instead of coupling across a layer, we couple vertically along a slice. we `Minimize coupling` `between slices`, and `maximize coupling` `in a slice`.

With this approach, each of our vertical slices can decide for itself how to best fulfill the request. New features only add code, we're not changing shared code and worrying about side effects.

![](./assets/Vertical-Slice-Architecture.jpg)

In CQRS, we cutting our model and API into vertical slices. Each command/query handler is a separate silo. This is where you can reduce coupling between layers. Each handler can be a separated code unit, even copy/pasted. Thanks to that, we can tune down the specific method to not follow general conventions (e.g. use custom SQL query or even different storage). In a traditional layered architecture, when we change the core generic mechanism in one layer, it can impact all methods. 
