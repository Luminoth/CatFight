SHANE
==========

SPECIAL CONTROLLER HOOKUP

SPECIAL ANIMATIONS

FighterManager should parent and cleanup all of the pooled objects fighters spawn (ammo, impacts, specials, etc)

******* Controller needs to reset all of its state when going back to the Lobby screen

Instead of having Set/Clear slot messages, controllers should set that information
in their custom device state so that they get it back when they reconnect
and then the game doesn't have to do any extra broadcast work when receiving them

*SLOTS NEED TO BE NON-ABSTRACT IN ORDER TO SERIALIZE IN THE EDITOR

Fighter needs concrete set of actions - Move, Attack - which do stuff based on equipped items


TIEN
==========
