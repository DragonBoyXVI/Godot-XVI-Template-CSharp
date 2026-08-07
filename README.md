# XVI Template CSharp

This is a personal template project ive made public for others to use if they
wish.
This project is written wholey in CSharp, a GDScript counterpart can be found
in a diffrent tepo (XVI-Template).
DO NOT have both this templates in the same project! They have conflicting
names, and the assets are duped between them. Having both is redundant.

Also every script where has been writen with GDScript access in mind, in all
places that make sense at least.
Scripts like RayDict and Property cannot be accessed via GDScript,
so counterparts have been provided.

## Translation importer

This class allows parsing json files for translations, rather than using
Godot's CSV system.

This also allows storing other data like arrays, that can be retrived from
the importer class. For example, you could store an array of messages, then
pick one at random. Allowing you to have diffent messages, and a diffent 
number of messages per language.

Another use case is allowing users to make their own translations, that can be
imported and used to add new languages, or change an existing one.
None of this will change your translations files, as this class NEVER writes
to a file.

## Drawing shape nodes

These arent the most useful, mostly just a carry over from some experements.
They draw simple shapes, might add more =3

## Stripped nodes

These nodes have some properties stripped from them.
This is something i like to do to prevent editing of properties that are set
via code (e.g. Setting the CollisionLayer and CollisionMask of a CharacterBody2D).

## State machines

A node based state machine implementation, generalized for most use cases.