 The application was implemented with [.NET Core 3.1 SDK](https://github.com/dotnet/core/tree/master/release-notes/3.1) and [.NET Standard 2.0](https://docs.microsoft.com/en-us/dotnet/standard/net-standard).

## How to run locally? ##

#### Run the application as a whole.

After running the command below, the API can be accessed via http://localhost:3015/
```bash

docker-compose -f docker-compose.yml up --remove-orphans --force-recreate --renew-anon-volumes --build --abort-on-container-exit

```

#### The datastore for local development

```bash

docker-compose -f docker-compose.local.yml up --remove-orphans --force-recreate --renew-anon-volumes --build --abort-on-container-exit

```

## Decisions on the development side ##
 
1. The API provides data from the data store. The API serves as a read model.
  2. The console application that feeds the data store from the external [Maze TV API](http://api.tvmaze.com).

The application was developed as a [modular monolith](https://learning.oreilly.com/library/view/monolith-to-microservices/9781492047834/). Due to the load and complexity of the system, it can easily be segregated in the near future if necessary to separate microservices.

The [StyleCop](https://github.com/StyleCop/StyleCop) was used to have a code style similar to the [Microsoft official ruleset](https://docs.microsoft.com/en-us/visualstudio/code-quality/code-analysis-for-managed-code-warnings?view=vs-2019). Unfortunately, I didn't have time to properly follow the rules (especially to document the important code blocks).

### [The API](https://github.com/saddambilalov/Rtl.Assignment/tree/master/src/Api)
Based on the above decision, the [API abstraction layer](https://github.com/saddambilalov/Rtl.Assignment/tree/master/src/Api/Rtl.Assignment.Api.Abstractions/) was created to facilitate possible integration into the system. It only contains request and response models.

The API was documented with the **Open API (Swagger)** support.

### [Console Scraper](https://github.com/saddambilalov/Rtl.Assignment/tree/master/src/Console/Rtl.Assignment.Scraper)
When calling the external API the issues were handled with Polly [Advanced](https://github.com/App-vNext/Polly/wiki/Advanced-Circuit-Breaker) (to have a circuit breaker) and [Jitter](https://github.com/App-vNext/Polly/wiki/Retry-with-jitter) (to avoid retries bunching into further spikes of load) retry policies. To make external integration easier and clearer, Rifl was used.

When the console application starts, shows are fetched from the defined page to the defined page. It's part of the [configuration](https://github.com/saddambilalov/Rtl.Assignment/blob/7777b5e2d03059ddb6c1ca3d3dfd7c095447e19d/src/Console/Rtl.Assignment.Scraper/appsettings.json#L15) and was also passed as an environment variable when [docker-compose](https://github.com/saddambilalov/Rtl.Assignment/blob/7777b5e2d03059ddb6c1ca3d3dfd7c095447e19d/docker-compose.yml#L11) runs for demo purposes.

Even with the same collection being used to store data, I tried to avoid merging data from the different API endpoints which could cause a problem if one endpoint is reachable and another is not. Although, the **MediatoR** pattern has helped to separate the data enrichments **cast** and **show**. A broker ([RabbitMq]([https://www.rabbitmq.com/](https://www.rabbitmq.com/)), [Kafka]([https://kafka.apache.org/](https://kafka.apache.org/)), etc.). can later be used to completely separate the enrichments for better **scalability** (more than one listener can enrich the shows) and **data consistency** (if the listener crashes the process can be continued by the other).  

### [Domain Model Layer](https://github.com/saddambilalov/Rtl.Assignment/tree/master/src/Rtl.Assignment.Domain)
It resesents the concerpt of business and follows the [Persistence Ignorance](https://deviq.com/persistence-ignorance/) and the [Infrastructure Ignorance](https://ayende.com/blog/3137/infrastructure-ignorance) principles. The entities at this layer are [POCO](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/ddd-oriented-microservice) and do not contain an attribute.

### [The infrastructure layer](https://github.com/saddambilalov/Rtl.Assignment/tree/master/src/Rtl.Assignment.Infrastructure)
This layer contains the implementation of data persistence and repository.

## Note. There are some important cases left in order to be discussed in the interview: ##
1 .Instead of feeding data from the external API, it can be better to have the publish/subscriber pattern. When something has changed on the show and cast side they can publish an event that will be executed by a subscription in the reading model. It can also be the same as the [CQRS](https://learning.oreilly.com/library/view/designing-event-driven-systems/9781492038252/ch07.html) implementation.
2. The application was not covered with the unit tests as a whole due to the time limitation.
3. And so on...
