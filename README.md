**The process of launching locally**
To run the application locally, follow these steps:

1) Download the project from the repository.
2) Open the project in your chosen development environment.
3) Run the main application file.

**This project has this:**
- game against Man
- white starts
- there is a choice of colors for the teams
- there is a scoreboard of how many chips are left
- a scoreboard showing whose turn it is.
- no kick back
- have a queen
**Design Patterns:**

**1) Observer**

A) [ColorCB_SelectedIndexChanged](CheckersGame/PrototypeCheckerGame.cs#L52): This method observes changes to the shape color selection combo boxes. When the user selects a new color, the SelectedIndexChanged event is called and this method handles the color selection.

B) [OnFigurePress](CheckersGame/PrototypeCheckerGame.cs#L237): This method observes the event of clicking a shape button on the playfield. When the user clicks the button, the Click event is called, and this method handles the pressing of the figure.


**2) Factory Method**

[GetSelectedImage](CheckersGame/PrototypeCheckerGame.cs#L64): This method is used to create images for different colors of shapes depending on the user-selected value in the combo boxes. It acts as a factory method that creates and returns an image depending on the selected color.


**3) Strategy**
The [ColorCB_SelectedIndexChanged](CheckersGame/PrototypeCheckerGame.cs#L52), [UpdateColorManager](CheckersGame/PrototypeCheckerGame.cs#L80), and UpdateButtonImages methods are used to select the colors of shapes depending on the user-selected value in the combo boxes. These methods allow you to easily add new shape colors without changing the underlying code.



**4) Template Method**
Method [Initialization](CheckersGame/PrototypeCheckerGame.cs#L150): This method defines the basic algorithm for initializing the game. It uses the template method because it defines a general initialization algorithm, and specific steps (such as creating a playfield) can be implemented in child classes or overridden if necessary.



**Programming Principles**
**1) DRY (Don't Repeat Yourself)**
Methods such as [ShowDiagonalWayUpRight](CheckersGame/PrototypeCheckerGame.cs#L348), [ShowDiagonalWayUpLeft](CheckersGame/PrototypeCheckerGame.cs#L367), [ShowDiagonalWayDownLeft](CheckersGame/PrototypeCheckerGame.cs#L386), and [ShowDiagonalWayDownRight](CheckersGame/PrototypeCheckerGame.cs#L405) demonstrate this principle by encapsulating similar logic for mapping diagonal paths into separate methods and thus reducing redundancy.


**2) KISS (Keep It Simple, Stupid)**
The logic in methods such as [HandleValidPress](CheckersGame/PrototypeCheckerGame.cs#L260) and [HandleInvalidPress](CheckersGame/PrototypeCheckerGame.cs#L284) is simple and straightforward, focusing on the current task without excessive complexity.

**3) SOLID:**

**A) SRP**
Methods are designed with sole responsibility, such as [ShowDiagonalWayUpRight](CheckersGame/PrototypeCheckerGame.cs#L348), which is responsible for displaying the diagonal path for a particular direction.

**B) OCP**
The code is open to extension but closed to modification, allowing for future additions without significantly altering existing code.

**C) DIP**
The code depends on abstractions rather than concrete implementations, which promotes flexibility and ease of change.


**4) Composition Over Inheritance**
The use of composition can be seen in how the [CheckerColorManager](CheckersGame/CheckerColorManager.cs#L348) class is used to manage checker colors, providing flexibility and encapsulation.



**Refactoring Techniques**

**1) Extract Method:** Separating a section of code into a separate method to improve readability and reuse.

**Example:**[ShowDiagonalWayUpRight](CheckersGame/CheckerColorManager.cs#L348) 

**2)Extract Variable:** Creating a variable to temporarily store a value, improve clarity, and reduce code duplication.
**Example:**[ShowDiagonalWayUpRight](CheckersGame/CheckerColorManager.cs#L348) 

**3)Remove Duplicate Code:** Getting rid of repetitive sections of code by placing them in separate methods or classes.

**Example:**[GetSelectedImage](CheckersGame/CheckerColorManager.cs#L64) 
**4)Improve Variable and Method Names:** Selecting informative and clear names for variables and methods to improve code readability.
**Example:**[DamkaModActivated](CheckersGame/CheckerColorManager.cs#L643) 

**5)Split Method:** Splitting a large method into smaller methods to improve code structure and readability.
**Example:**[ShowDiagonalWayUpRight](CheckersGame/CheckerColorManager.cs#L334) 



