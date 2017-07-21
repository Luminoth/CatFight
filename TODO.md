SHANE
==========

*SLOTS NEED TO BE NON-ABSTRACT IN ORDER TO SERIALIZE IN THE EDITOR

Master player is no longer updating on the controller lobby UI when the master player disconnects :(
PROGRAMMER UI THE CONTROLLER FOR NOW TO SET/CLEAR SLOTS AND SHIT

Need to handle items that take up multiple slots
Need to figure out how to build out weapons and specials and the brain, armor looks good tho

Items have behaviors that determine what happens when used (but then I removed items so I dunno)
Fighter needs concrete set of actions - Move, Attack - which do stuff based on equipped items
Get methods in place on the controller to set/clear schematic slots

Brain base types (which AI to use, pick democratically):
    aggressive
    defensive

Weapon base types:
    machinegun (rapid fire, low cooldown)
    laser (high impact, high cooldown)
    missile (AoE, average damage, medium cooldown)

Armor base types:
    light (low armor, no move mod)
    medium (medium armor, slight move mod)
    heavy (high armor, high move mod)
    power (high armor, no move mod, takes 2 slots)

Special base types:
    blastwave (radial AoE)


TIEN
==========
