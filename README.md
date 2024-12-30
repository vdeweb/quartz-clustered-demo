# Quartz Clustered Demo

Demo project which illustrates how to create a clustered job scheduler with Quartz.NET C#-library and PostgreSQL as persistent storage.

## The aim

The code in this repo can be used as a starting point when you need to create a job scheduler which has the following capabilities:
- it can run job with customizable triggers;
- there are many possible job producers which can schedule new jobs;
- there are many possible job consumers which can execute scheduled jobs;
- every job trigger event has to be executed only once (by a single job consumer).

In other words, we have a cluster with many job producers and job consumers, and we want to load-balance actions of job creation between many producers and actions of job execution between many consumers.

## Solution

Quartz.NET library offers enough capabilities to build a job scheduler that satisfies all necessary conditions. This project demonstrates how to properly configure Quartz.NET and how to use it in simple console applications with background services.

## Prerequisites

In this demo project PostgreSQL database is used as a persistent storage for the job scheduler. Quartz.NET will not create the database for its internal use so you have to create the database of required structure by yourself. Thankfully, there is a script in Quartz.NET repo on GitHub that does all necessary actions. It's recommended to take the script from the same release as the Quartz.NET Nuget package. The script for release v3.13.1 (which is used in the current demo project) can be found [here](https://github.com/quartznet/quartznet/blob/v3.13.1/database/tables/tables_postgres.sql).

## Sources overview

### QuartzClusteredDemo.Jobs

This project contains `ExampleJob` that is imported by both consumers and producers. Quartz.NET job definitions are designed to be shared between job consumers and job producers. `ExampleJob` doesn't do any meaningful work but generates a verbose output for demonstration purposes.

### QuartzClusteredDemo.JobProducer

Console application with background worker that periodically adds new jobs to schedule. You can run multiple instances of `JobProducer`.

### QuartzClusteredDemo.JobConsumer

Console application that reads schedule and executes jobs when they are supposed to be executed. You can run multiple instances of `JobConsumer`: every single job will be executed only by one consumer.

## Configuration

The key configuration of job producer and job consumer is stored in the file `quartz.json`. Please, check the connection string to your database in `quartz.json` before you start.

## Run the demo

You should start an instance (or multiple instances) of JobProducer to schedule some example jobs and an instance (or multiple instances) of JobConsumer to execute them. Each consumer executes two tasks in parallel with default settings.

## License

The code is distributed under MIT license. See `LICENSE.txt` for more information.

## Maintainer

[@vdeweb](https://github.com/vdeweb)

## Links

The demo project repo: [https://github.com/vdeweb/quartz-clustered-demo](https://github.com/vdeweb/quartz-clustered-demo)

Quartz.NET: [https://www.quartz-scheduler.net](https://www.quartz-scheduler.net)
