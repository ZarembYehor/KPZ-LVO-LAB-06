﻿using CheckersGame.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CheckersGame
{
    public partial class PrototypeCheckerGame : Form
    {
        const int MapSize = 8;
        const int CheckSize = 50;
        int currentPlayer;
        List<Button> simpleSteps = new List<Button>();
        Button[,] buttons = new Button[MapSize, MapSize];
        int countEatSteps = 0;
        Button prevButton;
        Button pressedButton;
        bool isContinue = false;

        bool isMoving;

        int[,] map = new int[MapSize, MapSize];

        Image UpFigure = GetThumbnailImage("white", CheckSize);
        Image DownFigure = GetThumbnailImage("black", CheckSize);

        private CheckerColorManager colorManager;
    
        public PrototypeCheckerGame()
        {
            InitializeComponent();

            ButtonExtensions.CheckSize = CheckSize;

            Image whiteDefault = GetThumbnailImage("white", CheckSize);
            Image blackDefault = GetThumbnailImage("black", CheckSize);

            colorManager = new CheckerColorManager(whiteDefault, blackDefault);

            UPColorCB.SelectedIndexChanged += ColorCB_SelectedIndexChanged;
            DOWNColorCB.SelectedIndexChanged += ColorCB_SelectedIndexChanged;

            this.Controls.Add(CurrentPlayerLabel);


            CurrentPlayerLabel.Text = "Now move Player 1";
            currentPlayer = 1;
            UpdateCurrentPlayerLabel();

            Initialization();
        }

        #region Callbacks
        // Сhecker methods
        private void ColorCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;

            Image selectedImage = GetSelectedImage(cb.SelectedItem.ToString());

            if (selectedImage != null)
            {
                UpdateColorManager(cb, selectedImage);
                UpdateButtonImages(cb);
            }
        }

        // Handler of the event of pressing on the figure
        public void OnFigurePress(object sender, EventArgs e)
        {
            if (prevButton != null)
                prevButton.BackColor = GetPrevButtonColor(prevButton);

            pressedButton = sender as Button;

            if (IsValidPress())
            {
                HandleValidPress();
            }
            else
            {
                HandleInvalidPress();
            }

            prevButton = pressedButton;
        }

        #endregion

        public static Image GetThumbnailImage(string color, int checkSize)
        {
            Dictionary<string, Image> colorImages = new Dictionary<string, Image>
            {
                { "white", Properties.Resources.white },
                { "black", Properties.Resources.black }
            };

            string normalizedColor = color.ToLowerInvariant();

            if (colorImages.ContainsKey(normalizedColor))
            {
                Image originalImage = colorImages[normalizedColor];

                return originalImage.GetThumbnailImage(checkSize - 10, checkSize - 10, null, IntPtr.Zero);
            }
            else
            {
                return null;
            }
        }

        private Image GetSelectedImage(string color)
        {
            Dictionary<string, Bitmap> colorImages = new Dictionary<string, Bitmap>
            {
                { "white", Properties.Resources.white },
                { "black", Properties.Resources.black },
                { "blue", Properties.Resources.blue },
                { "yellow", Properties.Resources.yellow }
            };

            string normalizedColor = color.ToLowerInvariant();

            if (colorImages.ContainsKey(normalizedColor))
            {
                Bitmap selectedImage = colorImages[normalizedColor];

                return selectedImage.GetThumbnailImage(CheckSize - 10, CheckSize - 10, null, IntPtr.Zero);
            }
            else
            {
                return null;
            }
        }

        private void UpdateColorManager(ComboBox cb, Image selectedImage)
        {
            if (cb == UPColorCB)
            {
                colorManager.WhiteFigure = selectedImage;
            }
            else if (cb == DOWNColorCB)
            {
                colorManager.BlackFigure = selectedImage;
            }
        }
        private void UpdateButtonImages(ComboBox cb)
        {
            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    int player = map[i, j];
                    if ((player == 1 && cb == UPColorCB) || (player == 2 && cb == DOWNColorCB))
                    {
                        colorManager.UpdateButtonImage(buttons[i, j], player);
                    }
                }
            }
        }
        public Color GetPrevButtonColor(Button prevButton)
        {
            if ((prevButton.GetBoardY() % 2) != 0)
            {
                if ((prevButton.GetBoardX() % 2) == 0)
                {
                    return Color.Gray;
                }
            }
            if (prevButton.GetBoardY() % 2 == 0)
            {
                if (prevButton.GetBoardX() % 2 != 0)
                {
                    return Color.Gray;
                }
            }
            return Color.White;
        }

        // Mechanics of counting the number of remaining checkers
        public int CountCheckers(int player)
        {
            int count = 0;
            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    if (map[i, j] == player)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
        public void UpdateCheckersCountLabels()
        {
            int whiteCheckersCount = CountCheckers(1);
            int blackCheckersCount = CountCheckers(2);

            UpperLabel.Text = $"P1: {whiteCheckersCount}/12";
            LowerLabel.Text = $"P2: {blackCheckersCount}/12";
        }

        // Methods for rendering
        public void Initialization()
        {
            currentPlayer = 1;
            isMoving = false;
            prevButton = null;

            map = new int[MapSize, MapSize] {

                { 0,1,0,1,0,1,0,1 },
                { 1,0,1,0,1,0,1,0 },
                { 0,1,0,1,0,1,0,1 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 2,0,2,0,2,0,2,0 },
                { 0,2,0,2,0,2,0,2 },
                { 2,0,2,0,2,0,2,0 }
            };
            CreateMap();
        }
        public void ResetGame()
        {
            bool player1 = false;
            bool player2 = false;

            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    if (map[i, j] == 1)
                        player1 = true;
                    if (map[i, j] == 2)
                        player2 = true;
                }
            }

            if (!player1 || !player2)
            {
                ClearGameBoard();
                Initialization();
            }
        }
        private void ClearGameBoard()
        {
            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    this.Controls.Remove(buttons[i, j]);
                }
            }
        }
        public void CreateMap()
        {
            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    Button button = CreateButton(i, j);
                    SetButtonImage(button, map[i, j]);
                    buttons[i, j] = button;
                    Controls.Add(button);
                }
            }
        }

        private Button CreateButton(int row, int col)
        {
            var button = new Button()
            {
                Location = new Point(col * CheckSize, row * CheckSize),
                Size = new Size(CheckSize, CheckSize),
                ForeColor = Color.Red,
                BackColor = GetPrevButtonColor(row, col)
            };

            button.Click += OnFigurePress;
            return button;
        }

        private void SetButtonImage(Button button, int value)
        {
            switch (value)
            {
                case 1:
                    button.Image = UpFigure;
                    break;
                case 2:
                    button.Image = DownFigure;
                    break;
                default:
                    button.Image = null;
                    break;
            }
        }

        private Color GetPrevButtonColor(int row, int col)
        {
            return (row + col) % 2 == 0 ? Color.White : Color.Black;
        }
        private void SwitchPlayer()
        {
            currentPlayer = currentPlayer == 1 ? 2 : 1;
            ResetGame();
            UpdateCurrentPlayerLabel();
            CurrentPlayerLabel.ForeColor = currentPlayer == 1 ? Color.Red : Color.Blue;
        }
        private void UpdateCurrentPlayerLabel()
        {
            CurrentPlayerLabel.Text = $"Now move Player {currentPlayer}";
        }

        
        private bool IsValidPress()
        {
            return (pressedButton != null &&
                    map[pressedButton.GetBoardY(), pressedButton.GetBoardX()] == currentPlayer);
        }
        private void HandleValidPress()
        {
            CloseSteps();
            pressedButton.BackColor = Color.Red;
            DeactivateAllButtons();
            pressedButton.Enabled = true;
            countEatSteps = 0;

            bool isKing = pressedButton.Text == "👑";

            ShowStepsWay(pressedButton.GetBoardY(), pressedButton.GetBoardX(), !isKing);

            if (isMoving)
            {
                CloseSteps();
                pressedButton.BackColor = GetPrevButtonColor(pressedButton);
                ShowPossibleSteps();
                isMoving = false;
            }
            else
            {
                isMoving = true;
            }
        }
        private void HandleInvalidPress()
        {
            if (isMoving)
            {
                isContinue = false;
                if (Math.Abs(pressedButton.GetBoardX() - prevButton.GetBoardX()) > 1)
                {
                    isContinue = true;
                    DeleteFallenChekers(pressedButton, prevButton);
                }
                MoveButtons();
            }
        }
        private void MoveButtons()
        {
            int temp = map[pressedButton.GetBoardY(), pressedButton.GetBoardX()];
            map[pressedButton.GetBoardY(), pressedButton.GetBoardX()] = map[prevButton.GetBoardY(), prevButton.GetBoardX()];
            map[prevButton.GetBoardY(), prevButton.GetBoardX()] = temp;
            pressedButton.Image = prevButton.Image;
            prevButton.Image = null;
            pressedButton.Text = prevButton.Text;
            prevButton.Text = "";
            DamkaModActivated(pressedButton);
            countEatSteps = 0;
            isMoving = false;
            CheckForWin();
            CloseSteps();
            DeactivateAllButtons();
            HandleContinuation();
            UpdateCheckersCountLabels();

        }
        private void HandleContinuation()
        {
            if (countEatSteps == 0 || !isContinue)
            {
                CloseSteps();
                SwitchPlayer();
                ShowPossibleSteps();
                isContinue = false;
            }
            else if (isContinue)
            {
                pressedButton.BackColor = Color.Red;
                pressedButton.Enabled = true;
                isMoving = true;
            }
        }

        // Displays possible moves
        public void ShowStepsWay(int iCurrFigure, int jCurrFigure,bool isOnestep = true)
        {
            simpleSteps.Clear();
            ShowDiagonalWay(iCurrFigure, jCurrFigure,isOnestep);
            if (countEatSteps > 0)
                CloseSimpleSteps(simpleSteps);
        }
        public void ShowDiagonalWay(int IcurrFigure, int currentFigureColumn, bool isOneStep = false)
        {
            ShowDiagonalWayUpRight(IcurrFigure, currentFigureColumn, isOneStep);
            ShowDiagonalWayUpLeft(IcurrFigure, currentFigureColumn, isOneStep);
            ShowDiagonalWayDownLeft(IcurrFigure, currentFigureColumn, isOneStep);
            ShowDiagonalWayDownRight(IcurrFigure, currentFigureColumn, isOneStep);
        }
        private void ShowDiagonalWayUpRight(int currentFigureRow, int currentFigureColumn, bool isOneStep)
        {
            int j = currentFigureColumn + 1;
            for (int i = currentFigureRow - 1; i >= 0; i--)
            {
                if (currentPlayer == 1 && isOneStep && !isContinue) break;
                if (IsInsideBorders(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j < 7)
                    j++;
                else break;

                if (isOneStep)
                    break;
            }
        }
        private void ShowDiagonalWayUpLeft(int currentFigureRow, int currentFigureColumn, bool isOneStep)
        {
            int j = currentFigureColumn - 1;
            for (int i = currentFigureRow - 1; i >= 0; i--)
            {
                if (currentPlayer == 1 && isOneStep && !isContinue) break;
                if (IsInsideBorders(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j > 0)
                    j--;
                else break;

                if (isOneStep)
                    break;
            }
        }
        private void ShowDiagonalWayDownLeft(int currentFigureRow, int currentFigureColumn, bool isOneStep)
        {
            int j = currentFigureColumn - 1;
            for (int i = currentFigureRow + 1; i < 8; i++)
            {
                if (currentPlayer == 2 && isOneStep && !isContinue) break;
                if (IsInsideBorders(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j > 0)
                    j--;
                else break;

                if (isOneStep)
                    break;
            }
        }
        private void ShowDiagonalWayDownRight(int currentFigureRow, int currentFigureColumn, bool isOneStep)
        {
            int j = currentFigureColumn + 1;
            for (int i = currentFigureRow + 1; i < 8; i++)
            {
                if (currentPlayer == 2 && isOneStep && !isContinue) break;
                if (IsInsideBorders(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j < 7)
                    j++;
                else break;

                if (isOneStep)
                    break;
            }
        }
        public void ShowProceduralDead(int i,int j,bool isOneStep = true)
        {
            int dirX = i - pressedButton.GetBoardY();
            int dirY = j - pressedButton.GetBoardX();
            dirX = dirX < 0 ? -1 : 1;
            dirY = dirY < 0 ? -1 : 1;
            int il = i;
            int jl = j;
            bool isEmpty = true;
            while (IsInsideBorders(il, jl))
            {
                if (map[il, jl] != 0 && map[il, jl] != currentPlayer)
                { 
                    isEmpty = false;
                    break;
                }
                il += dirX;
                jl += dirY;

                if (isOneStep)
                    break;
            }
            if (isEmpty)
                return;
            List<Button> toClose = new List<Button>();
            bool closeSimple = false;
            int ik = il + dirX;
            int jk = jl + dirY;
            while (IsInsideBorders(ik,jk))
            {
                if (map[ik, jk] == 0 )
                {
                    if (IsButtonHasDeleteStep(ik, jk, isOneStep, new int[2] { dirX, dirY }))
                    {
                        closeSimple = true;
                    }
                    else
                    {
                        toClose.Add(buttons[ik, jk]);
                    }
                    buttons[ik, jk].BackColor = Color.Yellow;
                    buttons[ik, jk].Enabled = true;
                    countEatSteps++;
                }
                else break;
                if (isOneStep)
                    break;
                jk += dirY;
                ik += dirX;
            }
            if (closeSimple && toClose.Count > 0)
            {
                CloseSimpleSteps(toClose);
            }
            
        }
        public void ShowPossibleSteps()
        {
            bool isOneStep = true;
            bool isEatStep = false;
            DeactivateAllButtons();
            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    if (map[i, j] == currentPlayer)
                    {
                        if (buttons[i, j].Text == "👑")
                            isOneStep = false;
                        else isOneStep = true;
                        if (IsButtonHasDeleteStep(i, j, isOneStep, new int[2] { 0, 0 }))
                        {
                            isEatStep = true;
                            buttons[i, j].Enabled = true;
                        }
                    }
                }
            }
            if (!isEatStep)
                ActivateAllButtons();
        }

        // Closes possible moves
        public void CloseSteps()
        {
            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    buttons[i, j].BackColor = GetPrevButtonColor(buttons[i, j]);
                }
            }
        }
        public void CloseSimpleSteps(List<Button> simpleSteps)
        {
            if (simpleSteps.Count > 0)
            {
                for (int i = 0; i < simpleSteps.Count; i++)
                {
                    simpleSteps[i].BackColor = GetPrevButtonColor(simpleSteps[i]);
                    simpleSteps[i].Enabled = false;
                }
            }
        }

        // Checks if there are possible moves with eating pieces.
        public bool IsButtonHasDeleteStep(int IcurrFigure, int currentFigureColumn, bool isOneStep, int[] dir)
        {
            bool deleteStep = false;

            deleteStep = CheckDirection(IcurrFigure, currentFigureColumn, isOneStep, dir, 1, -1);
            deleteStep |= CheckDirection(IcurrFigure, currentFigureColumn , isOneStep, dir, 1, 1);
            deleteStep |= CheckDirection(IcurrFigure, currentFigureColumn, isOneStep, dir, -1, 1);
            deleteStep |= CheckDirection(IcurrFigure, currentFigureColumn, isOneStep, dir, -1, -1);

            return deleteStep;
        }
        private bool CheckDirection(int currentFigureRow, int currentFigureColumn, bool isOneStep, int[] dir, int deltaI, int deltaJ)
        {
            bool eatStep = false;
            int j = currentFigureColumn + deltaJ;
            for (int i = currentFigureRow + deltaI; i >= 0 && i < 8; i += deltaI)
            {
                if ((currentPlayer == 1 && isOneStep && !isContinue) || (dir[0] == deltaI && dir[1] == deltaJ && !isOneStep))
                    break;
                if (IsInsideBorders(i, j))
                {
                    if (map[i, j] != 0 && map[i, j] != currentPlayer)
                    {
                        eatStep = true;
                        if (!IsInsideBorders(i + deltaI, j + deltaJ))
                            eatStep = false;
                        else if (map[i + deltaI, j + deltaJ] != 0)
                            eatStep = false;
                        else
                            return eatStep;
                    }
                }
                j += deltaJ;
                if (isOneStep)
                    break;
            }
            return eatStep;
        }

        public bool IsInsideBorders(int x, int y) => x >= 0 && x < MapSize && y >= 0 && y < MapSize;

        // Other activities
        public void ActivateAllButtons()
        {
            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    buttons[i, j].Enabled = true;
                }
            }
        }
        public void DeactivateAllButtons()
        {
            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    buttons[i, j].Enabled = false;
                }
            }
        }
        public void DeleteFallenChekers(Button endButton, Button startButton)
        {
            int count = Math.Abs(endButton.GetBoardY() - startButton.GetBoardY());
            int startIndexX = endButton.GetBoardY() - startButton.GetBoardY();
            int startIndexY = endButton.GetBoardX() - startButton.GetBoardX();
            startIndexX = startIndexX < 0 ? -1 : 1;
            startIndexY = startIndexY < 0 ? -1 : 1;
            int currCount = 0;
            int i = startButton.GetBoardY() + startIndexX;
            int j = startButton.GetBoardX() + startIndexY;
            while (currCount < count - 1)
            {
                map[i, j] = 0;
                buttons[i, j].Image = null;
                buttons[i, j].Text = "";
                i += startIndexX;
                j += startIndexY;
                currCount++;
            }
            UpdateCheckersCountLabels();

        }
        public bool DeterminePath(int ti, int tj)
        {

            if (map[ti, tj] == 0 && !isContinue)
            {
                buttons[ti, tj].BackColor = Color.Yellow;
                buttons[ti, tj].Enabled = true;
                simpleSteps.Add(buttons[ti, tj]);
            }
            else
            {

                if (map[ti, tj] != currentPlayer)
                {
                    if (pressedButton.Text == "👑")
                        ShowProceduralDead(ti, tj, false);
                    else ShowProceduralDead(ti, tj);
                }

                return false;
            }
            return true;
        }
        public void DamkaModActivated(Button button)
        {
            if (map[button.GetBoardY(), button.GetBoardX()] == 1 && button.GetBoardY() == MapSize - 1)
            {
                button.Text = "👑";

            }
            if (map[button.GetBoardY(), button.GetBoardX()] == 2 && button.GetBoardY() == 0)
            {
                button.Text = "👑";
            }
        }

        // Congratulations to Players
        private void CheckForWin()
        {
            int whiteCheckersCount = CountCheckers(1);
            int blackCheckersCount = CountCheckers(2);

            if (whiteCheckersCount == 0)
            {
                ShowGameOverMessage("Congratulations! Player 2 has won!");
            }
            else if (blackCheckersCount == 0)
            {
                ShowGameOverMessage("Congratulations! Player 1 has won!");
            }
        }
        private void ShowGameOverMessage(string message)
        {
            MessageBox.Show(message, "Win", MessageBoxButtons.OK);
        }
    }
}