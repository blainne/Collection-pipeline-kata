# Collection pipeline kata
Code presented is an implementation for an imaginary online fantasy game statistics service. It exposes api methods that might be used to display some fancy charts and tables to the user.
The only executable part are tests that verify correctness of api implementation. You can use them to check whether the modifications done are correct. 
The goal is to refactor all public methods in GameStatsApi  class from basing mainly on loops to nice declarative code using collection pipeline methods.

There are also some additional assumptions (also included in the example code in the notes.txt file):
- All models have some unique identifying property and IEquatable implemented to simplify things.
- Assume that there are no shared characters among users

### Find more about collection pipeline ###
There are some good sources to read about new features:

* http://martinfowler.com/articles/collection-pipeline/ - An article from Martin Fowler explaining the concept. It has great cheat-sheet at the end.
* http://martinfowler.com/articles/refactoring-pipelines.html - Again from Martin Fowler. This time he goes through refactoring process that was an inspiration to this kata.
* https://michaelfeathers.silvrback.com/collection-pipelines-the-revenge-of-c - Short blog post by Michael Feathers on the concept.

