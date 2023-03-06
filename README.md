# Implement a Message Queue using Redis, Docker & .NET Core.

## Problem Description

The challenge consists of implementing a message queue using Redis and also implementing an API (it can be Flask) that allows us to abstract ourselves.
This API must be made up of 3 methods (endpoints) that must comply with the following contract:

### Pop:

    - Endpoint: /api/queue/pop
        - Method: POST
    - Response:
        - Status code: 200
        - Body: 
            {
              'status': 'ok',
              'message': <msg>
            }

### Push:

    - Endpoint: /api/queue/push
        - Method: POST
    - Body: <msg>
    - Response:
        - Status code: 200
          - Body:
              {
              'status': 'ok'
              }

### Number of messages:

    - Endpoint: /api/queue/count
    - Method: GET
    - Response:
        - Status code: 200
        - Body:
            {
            'status': 'ok',
            'count': <count>
            }

### Requirements:

  - Use docker.
  - Documentation of how it was implemented, if there are credentials, how to run it, details of implemented improvements.

## Solution

The challenge is a bit confusing because it mentions a message queue while asking to implement Pop and Push, which are Stack, not Queue, operations, or maybe you did it intentionally :sweat_smile:.
Anyway, in order to comply the requirements while trying to be faithful to the underlying data structure (a queue) the implementation works as follows:

- Because I'm not a Redis expert and given the unordered nature of the `key:value` pairs, I used a `SortedDictionary<DateTime, string>` to mimic a queue behavior, where the messages are saved in the order in they were received, thus, at low level, a push operation executes an enqueue, and pop executes a dequeue. This can be seen in the [QueueRepository](https://github.com/hacasco/hc-redis-message-queue/blob/master/RedisMessageQueue/RedisMessageQueue/Infrastructure/QueueRepository.cs) class.

- As expected, the implementation is totally transparent for the controller and high-level classes because I'm using DI principle through the [IQueueRepository](https://github.com/hacasco/hc-redis-message-queue/blob/master/RedisMessageQueue/RedisMessageQueue/Domain/Interfaces/IQueueRepository.cs) interface.

- The API performs some basic validation, Push returns a `400` error if the message is null, Pop returns a `404` if the queue is empty.

- Deployment is very straighforward, just run `docker compose up`. If needed, the `ConnectionString` value the _appsettings.json_ file before running the application.

- Testing is also included. Make sure to run `docker compose up` to create the redis instance before running the tests.

