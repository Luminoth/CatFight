SHANE
==========

Instead of having Set/Clear slot messages, controllers should set that information
in their custom device state so that they get it back when they reconnect
and then the game doesn't have to do any extra broadcast work when receiving them


*SLOTS NEED TO BE NON-ABSTRACT IN ORDER TO SERIALIZE IN THE EDITOR

Items have behaviors that determine what happens when used (but then I removed items so I dunno)
Fighter needs concrete set of actions - Move, Attack - which do stuff based on equipped items





weapons don't combine
    vote winner wins (random tie breaker) and gets stronger by the # of votes

armor is stacking per-type damage reduction

special is vote of missiles or chaffs
    amount is equal to the votes for that special
    anybody can use at any time, but reduces available amount








TIEN
==========
