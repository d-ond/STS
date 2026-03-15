# 01 - Getting Started

**Date:** 2025-03-14

## What I worked on

Setting the project up and understanding the basics of the loop. Some parts are simply hard-coded via the use of a dummy (who doesn't attack). The player does not have complicated card setups or combos, and statuses are not set up yet. 

## Key decisions

- Using the Command pattern for card execution
- Separating the deck into four piles: draw, hand, discard, exhaust
- Cards carry their own data (damage, block, cost)

## What I learned

- The cards contain their own data - this is essentially the biggest point. Rather than the card itself being the "action", it's less coupled and can be a set of properties that are used by the card class. The action has some overlap in the data points, but it does not need the card itself, meaning that the execution and the deck aspects of the card can be separated. It opens up a few blockers and makes the code easier to work with - less hard-coded aspects. 

## Next steps

Statuses and card effects will need to become more complicated, and thus the effects of items will need to be taken into account to make them more streamlined and useable. 

At the moment, the cards and actions will be grouped into one single "effect" action command, which will extend to potions. 
