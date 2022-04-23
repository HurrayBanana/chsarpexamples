using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Runtime.Serialization;
using System.Text;

namespace Engine
{
    /// <summary>
    /// holds and manipulates high scores
    /// </summary>
#if !XBOX
    [Serializable]
#endif
    public class HighScore
    {

        /// <summary>
        /// defines an attribute of the highscore table
        /// (each represents a column of data)
        /// </summary>
#if !XBOX
        [Serializable]
#endif
        public class Column
        {
            /// <summary>
            /// The name of the attribute
            /// </summary>
            public String Name;
            /// <summary>
            /// The data type represented by the attribute (integer, string etc...)
            /// </summary>
            public String DataType;
            /// <summary>
            /// sets maximum length for the column based on example given
            /// </summary>
            public int MaxLength;
        }

        /// <summary>
        /// holds the sorting order for this highscore table
        /// defaults to highscore table
        /// </summary>
        private scoreOrder _scoreOrder = scoreOrder.HighScore;
        /// <summary>
        /// gets or sets the order of the score table, by default this is set to HighScore so biggest
        /// value will appear at top, but if doing a quickest time based score system then you need to
        /// set this to a LowScore
        /// </summary>
        public scoreOrder ScoreOrder
        {
            get
            {
                return _scoreOrder;
            }

            set
            {
                _scoreOrder = value;
            }
        }
        /// <summary>
        /// holds the highscore information
        /// </summary>
        private List<List<Object>> _scoreTable = new List<List<object>>();

        /// <summary>
        /// Holds a list of the names of each attribute
        /// to make it easy to extract individual columns
        /// </summary>
        private List<Column> _attributes = new List<Column>();
        /// <summary>
        /// holds the maximum digits that can be displayed
        /// </summary>
        private int _maxDigits = 7;
        /// <summary>
        /// holds the maximum size of the score table
        /// </summary>
        private int _max = 5;

        /// <summary>
        /// Creates a highscore table
        /// </summary>
        public HighScore()
        {
        }
        /// <summary>
        /// Allows you to specify either a low score or high score table
        /// </summary>
        /// <param name="so">Specify either a low score or high score table</param>
        public HighScore(scoreOrder so)
        {
            _scoreOrder = so;
        }
        /// <summary>
        /// Generates an empty table
        /// Does not alter any attributes
        /// </summary>
        public void ClearTable()
        {
            _scoreTable = new List<List<object>>();
        }
        /// <summary>
        /// gets or sets the maximum number of digits to display for scores
        /// </summary>
        private int MaximumDigits { get { return _maxDigits; } set { _maxDigits = value; } }

        /// <summary>
        /// Gets the top entry for that particualr column
        /// you will need to cast the object to use it (string)Top(1) if this is the name
        /// </summary>
        /// <param name="column">column value to return</param>
        /// <returns>entry requested from the top of the table</returns>
        public object Top(int column)
        {
            if (column < ColumnCount)
                return _scoreTable[0][column];
            return null;
        }
        /// <summary>
        /// Gets the Bottom entry for that particualr column
        /// you will need to cast the object to use it (int)Bottom(0) if this is the score
        /// </summary>
        /// <param name="column">column value to return</param>
        /// <returns>entry requested from bottom of table</returns>
        public object Bottom(int column)
        {
            if (column < ColumnCount)
                return _scoreTable[_scoreTable.Count - 1][column];
            return null;
        }
        /// <summary>
        /// gets the top score from the score table, if your scores are bigger than 2 billion then use TopScoreLarge
        /// </summary>
        /// <value>Returns an int value representing the score or -1 if no scores exist</value>
        public int TopScore
        {
            get
            {
                return (int)TopScoreLarge;
            }
        }
        /// <summary>
        /// gets the top score from the table in full 64bit integer format
        /// </summary>
        public Int64 TopScoreLarge
        {
            get
            {
                if (_scoreTable.Count > 0)
                {
                    //if (_scoreOrder == scoreOrder.HighScore)
                    return Convert.ToInt64(_scoreTable[0][0]);
                    //else
                    //    return Convert.ToInt64(_scoreTable[_scoreTable.Count - 1][0]);
                }
                return -1;
            }
        }

        /// <summary>
        /// gets the bottom score from the score table, if your scores are bigger than 2 billion then use BottomScoreLarge
        /// </summary>
        /// <value>Returns an int value representing the score or -1 if no scores exist</value>
        public int BottomScore
        {
            get
            {
                return (int)BottomScoreLarge;
            }
        }
        /// <summary>
        /// gets the bottom score from the table in full 64bit integer format
        /// </summary>
        public Int64 BottomScoreLarge
        {
            get
            {
                if (_scoreTable.Count > 0)
                {
                    //if (_scoreOrder == scoreOrder.HighScore)
                    return Convert.ToInt64(_scoreTable[_scoreTable.Count - 1][0]);
                    //else
                    //    return Convert.ToInt64(_scoreTable[0][0]);
                }
                return -1;
            }
        }

        /// <summary>
        /// gets or sets the maximum number of entries in the table
        /// </summary>
        public int MaximumEntries { get { return _max; } set { _max = value; } }

        /// <summary>
        /// gets or sets the entire score table for manual processing and serialisation?
        /// </summary>
        public List<List<Object>> TableDetails { get { return _scoreTable; } set { _scoreTable = value; } }
        /// <summary>
        /// gets or sets the Attribute list for manual processing 
        /// </summary>
        public List<Column> ColumnDetails { get { return _attributes; } set { _attributes = value; } }

        /// <summary>
        /// returns the number of entries in the table
        /// </summary>
        public int Entries { get { return _scoreTable.Count; } }

        /// <summary>
        /// returns the number of Rows entries in the table
        /// </summary>
        public int RowCount { get { return _scoreTable.Count; } }

        /// <summary>
        /// gets the number of attributes defined for the score table
        /// </summary>
        /// <value>A number representing the number of columns available in the table</value>
        public int ColumnCount { get { return _attributes.Count; } }

        ///// <summary>
        ///// gets the entire score table for direct processing, this is read only
        ///// </summary>
        //public List<List<object>> RawTable
        //{
        //    get
        //    {
        //        return _scoreTable;
        //    }
        //}



        /// <summary>
        /// defines an attribute for the higscores
        /// </summary>
        /// <param name="columnName">The name you wish to give this attribute (column of data)</param>
        /// <param name="exampleMaximum">An example of the maximum calue of the data that will be stored in this attribute</param>
        /// <remarks>The method needs an example of the data so it can record its base data type.
        /// This allows further methods to validate the score entries as they are given</remarks>
        /// <example>This example show you how to create a high score object
        /// and add an initial attribute to to it, assumes that an instance of HighScore
        /// has already been created and called highScoreTable in GameValues.
        /// <code>HighScore highScoreTable = new HighScoreTable();</code>
        /// Here is the example.
        /// <code>
        ///         /// &lt;summary&gt;
        ///         /// Attempt to load the highscores from the save file
        ///         /// or set defaults if it doesn't exist
        ///         /// &lt;/summary&gt;
        ///         public void DefineHighScore()
        ///         {
        ///                 //set the default scores
        ///                 //set attributes and maximum size
        ///                 highScoreTable.MaximumEntries = 3;
        ///                 //first attribute must be a score give a name and an example
        ///                 highScoreTable.DefineColumn("score", 1000);
        ///                 //the second attribute is called name, the example is HB
        ///                 highScoreTable.DefineColumn("name", "H B");
        /// 
        ///                 // create an individual highscore entry
        ///                 List&lt;Object&gt; scoreEntry = new List&lt;Object&gt;();
        /// 
        ///                 // add the highscore to the table
        ///                 scoreEntry.Add(2000);
        ///                 scoreEntry.Add("HB");
        ///                 highScoreTable.AddScore(scoreEntry);
        /// 
        ///                 // add the next score entry
        ///                 scoreEntry = new List&lt;Object&gt;();
        ///                 scoreEntry.Add(1500);
        ///                 scoreEntry.Add("ED");
        ///                 highScoreTable.AddScore(scoreEntry);
        /// 
        ///                 // add the next score entry
        ///                 scoreEntry = new List&lt;Object&gt;();
        ///                 scoreEntry.Add(1000);
        ///                 scoreEntry.Add("EZY");
        ///                 highScoreTable.AddScore(scoreEntry);
        ///         }
        /// </code>
        /// </example>
        public void DefineColumn(String columnName, Object exampleMaximum)
        {
            //force to Upper case
            columnName = columnName.ToUpper();
            //only add if it does not already exist
            if (NameExist(columnName) == -1)
            {
                Column newOne = new Column();
                newOne.Name = columnName;
                //not a very good way of doing this
                newOne.DataType = exampleMaximum.GetType().ToString();
                newOne.MaxLength = exampleMaximum.ToString().Length;
                _attributes.Add(newOne);
            }
        }

        /// <summary>
        /// Checks attribute names to see if given name exists
        /// </summary>
        /// <param name="name">The attribute name we want to search for</param>
        /// <returns>True if found, false is blank or not found</returns>
        private int NameExist(string name)
        {
            if (name == "")
                return -1;
            int outcome = -1;
            for (int i = 0; i < _attributes.Count; i++)
            {
                if (name == _attributes[i].Name)
                {
                    outcome = i;
                    break;
                }
            }
            return outcome;
        }

        /// <summary>
        /// adds a new score to the high score list
        /// </summary>
        /// <param name="newScore">A list of objects that make up the attributes for the score entry</param>
        /// <returns>the position score inserted into high score table if added successfully,
        /// -1 if an error occured or the position after the list one in the list, if the score wasn't high enough</returns>
        /// <remarks>If the </remarks>
        /// <example>This example show you how to create a high score object
        /// and add an initial attribute to to it
        /// <code>
        ///         /// &lt;summary&gt;
        ///         /// Attempt to load the highscores from the save file
        ///         /// or set defaults if it doesn't exist
        ///         /// &lt;/summary&gt;
        ///         public void DefineHighScore()
        ///         {
        ///                 //set the default scores
        ///                 //set attributes and maximum size
        ///                 highScoreTable.MaximumEntries = 3;
        ///                 //first attribute must be a score give a name and an example
        ///                 highScoreTable.DefineColumn("score", 1000);
        ///                 //the second attribute is called name, the example is HB
        ///                 highScoreTable.DefineColumn("name", "H B");
        /// 
        ///                 // create an individual highscore entry
        ///                 List&lt;Object&gt; scoreEntry = new List&lt;Object&gt;();
        /// 
        ///                 // add the highscore to the table
        ///                 scoreEntry.Add(2000);
        ///                 scoreEntry.Add("HB");
        ///                 highScoreTable.AddScore(scoreEntry);
        /// 
        ///                 // add the next score entry
        ///                 scoreEntry = new List&lt;Object&gt;();
        ///                 scoreEntry.Add(1500);
        ///                 scoreEntry.Add("ED");
        ///                 highScoreTable.AddScore(scoreEntry);
        /// 
        ///                 // add the next score entry
        ///                 scoreEntry = new List&lt;Object&gt;();
        ///                 scoreEntry.Add(1000);
        ///                 scoreEntry.Add("EZY");
        ///                 highScoreTable.AddScore(scoreEntry);
        ///         }
        /// </code>
        /// </example>
        public int AddScore(List<Object> newScore)
        {
            //need to re-write using ScoreCheck so one standard interface
            //for locating 
            if (!ScoreValid(newScore))
                return -1;
            int i = ScoreCheck(newScore[0]);
            if (i != -1)
            {
                _scoreTable.Insert(i, newScore);
                //remove any scores dropped off
                if (_scoreTable.Count > _max)
                    _scoreTable.RemoveAt(_max);
                return i;
            }
            else
                return i;
        }

        /// <summary>
        /// Verifies whether a score meets the types specifed
        /// </summary>
        /// <param name="newScore">The score to be validated</param>
        /// <returns>True to say it is a valid score or false to say it doesn't match</returns>
        private bool ScoreValid(List<object> newScore)
        {
            if (newScore.Count != _attributes.Count)
                return false;
            //skip the first column as this is the numerical score and we might be comparing 32bit and 64bit sizes
            for (int i = 1; i < _attributes.Count; i++)
                //pretty lame need to do type checking really
                if (newScore[i].GetType().ToString() != _attributes[i].DataType)
                    return false;
            return true;
        }

        /// <summary>
        /// Gets a specific highscore entry from the table
        /// </summary>
        /// <param name="row">This position in the table we wish to extract scores from
        /// the position starts at 0 for the first one and so on</param>
        /// <returns>A list containing the attributes for the selected row in the score table</returns>
        /// <remarks>If no score appears at the position given the return object will be null
        /// this should be checked for</remarks>
        public List<Object> GetRowObject(int row)
        {
            if (row < 0 || row > _scoreTable.Count - 1)
                return null;
            return _scoreTable[row];
        }

        /// <summary>
        /// Gets a specific highscore entry from the table
        /// </summary>
        /// <param name="row">This position in the table we wish to extract scores from
        /// the position starts at 0 for the first one and so on</param>
        /// <param name="delimiter">The string to place between attributes of the score</param>
        /// <returns>A string containing the attributes for the selected score separated by delimiter given.
        /// If row doesn't exist then ERROR will be returned in the string</returns>
        /// <remarks>If no score appears at the position given the return object will be null
        /// this should be checked for</remarks>
        /// <example>This example shows how to retrieve using GetEntryString and Count 
        /// the scores to display using DrawText. 
        /// You would need to place this code inside your DrawText subroutine
        /// <code>
        /// for (int i = 0; i &lt; highScoreTable.Count; i++)
        ///     gameManager.DrawText("fontsheet", highScoreTable.RowReturnAsString(i, " - "),
        ///                                        300, 200 + i * 40, new TextProperty(0.6f, 1, Color.Blue));
        /// </code>
        /// The output would be something like this (based on other high score examples)
        /// <code>
        /// 2000 - HB
        /// 1500 - ED
        /// 1000 - EZY
        /// </code>
        /// </example>
        public String GetRowString(int row, string delimiter)
        {
            if (row < 0 || row > _scoreTable.Count - 1)
                return "ERROR";
            String s = "";
            for (int i = 0; i < _attributes.Count; i++)
            {
                if (i != _attributes.Count - 1)
                {
                    int j = IntHelper.Largest(0, _attributes[i].MaxLength/*_maxDigits*/ - _scoreTable[row][i].ToString().Length);
                    s += _scoreTable[row][i].ToString() + StringHelper.Pad(" ", j) + delimiter;
                }
                else
                    s += _scoreTable[row][i].ToString();
            }
            return s;
        }
        /// <summary>
        /// returns a formatted score row including a rank number (assumes the column is named RANK - 4 characters long)
        /// </summary>
        /// <param name="row">This position in the table we wish to extract scores from
        /// the position starts at 0 for the first one and so on</param>
        /// <param name="delimiter">The string to place between attributes of the score</param>
        /// <returns>A string containing the attributes for the selected score separated by delimiter given.
        /// If row doesn't exist then ERROR will be returned in the string</returns>
        /// <returns>A string containing the attributes for the selected score separated by delimiter given.
        /// If row doesn't exist then ERROR will be returned in the string</returns>
        public String GetRowStringRank(int row, string delimiter)
        {
            return (row + 1).ToString() + StringHelper.Pad(" ", 4 - (row + 1).ToString().Length) + delimiter + GetRowString(row, delimiter);
        }
        /// <summary>
        /// Returns a crudelly formated string containing the headings from
        /// the score table
        /// </summary>
        /// <param name="delimiter">String to pad the between the headings</param>
        /// <returns>A string of column headings</returns>
        public String GetColumnHeadingsString(string delimiter)
        {
            String s = "";
            for (int i = 0; i < _attributes.Count; i++)
            {
                if (i != _attributes.Count - 1)
                {
                    int j = IntHelper.Largest(0, _attributes[i].MaxLength/*_maxDigits*/ - _attributes[i].Name.Length);
                    s += _attributes[i].Name + StringHelper.Pad(" ", j) + delimiter;
                }
                else
                    s += _attributes[i].Name;
            }
            return s;
        }
        /// <summary>
        /// Checks to see if the score given would make the highscore table
        /// </summary>
        /// <param name="score">The score to be checked against the highscore table</param>
        /// <returns>the position the score would occupy in the table.
        /// 0 indicates 1st place, 1 indicates 2nd place, and so on, -1 indicates not good enough</returns>
        /// <example>This example checks a score when a game ends and calls GetName() sub which sets playerName 
        /// before adding the score if it is good enough.
        /// <code>
        /// if (highScoreTable.CheckScore(score) != -1)
        /// {
        ///     GetName();
        ///     //create a score entry object
        ///     List&lt;Object&gt; scoreEntry = new List&lt;Object&gt;();
        /// 
        ///     // add the highscore to the table
        ///     scoreEntry.Add(2000);
        ///     scoreEntry.Add(playerName);
        ///     highScoreTable.AddScore(scoreEntry);
        /// }
        /// StartTitle();
        /// </code></example>
        public int ScoreCheck(Int64 score)
        {
            int i = -1;
            Boolean found = false;
            while (!found && ++i < _scoreTable.Count)
            {
                if (_scoreOrder == scoreOrder.HighScore)
                    found = score > Convert.ToInt64(_scoreTable[i][0]);
                else
                    found = score < Convert.ToInt64(_scoreTable[i][0]);
            }
            if (i < _max) return i;
            else
                if (found) return i; else return -1;
        }
        /// <summary>
        /// Checks to see if the score given would make the highscore table
        /// </summary>
        /// <param name="score">The score to be checked against the highscore table</param>
        /// <returns>the position the score would occupy in the table.</returns>
        public int ScoreCheck(int score)
        {
            return ScoreCheck(Convert.ToInt64(score));
        }
        /// <summary>
        /// Checks to see if the score given would make the highscore table
        /// </summary>
        /// <param name="score">The score to be checked against the highscore table</param>
        /// <returns>the position the score would occupy in the table.</returns>
        private int ScoreCheck(object score)
        {
            try
            {
                return ScoreCheck(Convert.ToInt64(score));
            }
            catch
            {
                return -1;
            }
        }
        /// <summary>
        /// returns the value of an attribute at a specified position in string form
        /// </summary>
        /// <param name="row">the position in the table we wish to retrieve</param>
        /// <param name="columnName">The name of the attribute we want the value of</param>
        /// <returns>The value formatted as a string or ERROR if the name or row supplied were invalid</returns>
        public String ItemValueAsString(int row, String columnName)
        {
            int col = NameExist(columnName.ToUpper());
            if (col != -1 && row >= 0 && row < _scoreTable.Count)
                return _scoreTable[row][col].ToString();
            else
                return "ERROR";
        }//GetAttributeAsString(int, string)

        /// <summary>
        /// returns the value of an attribute at a specified position in string form
        /// </summary>
        /// <param name="row">the position in the table we wish to retrieve</param>
        /// <param name="column">The column (0 is first column) of the attribute we want the value of</param>
        /// <returns>The value formatted as a string or ERROR if the row or column supplied is invalid</returns>
        /// <example>This example demonstrates how to use the GetAttributeValueString and GetAttributeName
        /// to display the HighScoreTable using just the data contained within it.
        /// This makes this very flexable, if you change any attribute names they will automatically be displayed
        /// correctly
        /// <code>
        ///         /// &lt;summary&gt;
        ///         /// Displays the HighScore table by using the row and column accessors
        ///         /// &lt;/summary&gt;
        ///         /// &lt;remarks&gt;Firstly displays the names of all the attributes then the rest of the table&lt;/remarks&gt;
        ///         public void DrawHighScores()
        ///         {
        ///             //display each of the attribute names (columns) spaced 100 pixels apart (horizontally)
        ///             //in white
        ///             for (int j = 0; j &lt; highScoreTable.Columns; j++)
        ///                 gameManager.DrawText("fontsheet", 
        ///                     highScoreTable.GetAttributeName(j), 300 + j*100, 200, 0.5f, 1, Color.White);
        /// 
        ///             //go through each column of the score table and display the values for each attribute
        ///             //j loop represents the columns and the i loop represents each row
        ///             //each column is 100 pixels apart and each row is 40 pixels apart
        ///             for (int j = 0; j &lt; highScoreTable.Columns; j++)
        ///                 for (int i = 0; i &lt; highScoreTable.Count; i++)
        ///                 {
        ///                     gameManager.DrawText("fontsheet", highScoreTable.GetAttributeValueAsString(i, j)
        ///                         , 300 + j * 100, 250 + i * 40, 0.5f, 1, Color.CornflowerBlue);
        ///             }
        ///         }
        /// </code>
        /// </example>
        public String ItemValueAsString(int row, int column)
        {
            if (column >= 0 && column < _attributes.Count && row >= 0 && row < _scoreTable.Count)
                return _scoreTable[row][column].ToString();
            else
                return "ERROR";
        }//GetAttributeAsString(int, int)

        /// <summary>
        /// returns the value of an attribute as an object
        /// </summary>
        /// <param name="row">the position in the table we wish to retrieve</param>
        /// <param name="columnName">The name of the attribute we want the value of</param>
        /// <returns>The value in its original form or null if the row or name specified was invalid</returns>
        public Object ItemValueAsObject(int row, String columnName)
        {
            int col = NameExist(columnName.ToUpper());
            if (col != -1 && row >= 0 && row < _scoreTable.Count)
                return _scoreTable[row][col];
            else
                return null;
        }//GetAttributeAsString(int, string)

        /// <summary>
        /// returns the value of an attribute as an object
        /// </summary>
        /// <param name="row">the position in the table we wish to retrieve</param>
        /// <param name="column">The name of the attribute we want the value of</param>
        /// <returns>The value in its original form or null if the row or column specified was invalid</returns>
        public Object ItemValueAsObject(int row, int column)
        {
            if (column >= 0 && column < _attributes.Count && row >= 0 && row < _scoreTable.Count)
                return _scoreTable[row][column];
            else
                return null;
        }//GetAttributeAsString(int, int)

        /// <summary>
        /// Gets the name of the specified column in the score table
        /// </summary>
        /// <param name="column">The column we want the name of</param>
        /// <returns>The name of the column or ERROR if the column specified was invalid</returns>
        public String ColumnName(int column)
        {
            if (column >= 0 && column < _attributes.Count)
                return _attributes[column].Name;
            else
                return "ERROR";
        }

        /// <summary>
        /// Returns a partially formatted table for debugging purposes
        /// </summary>
        /// <returns>a string formatted with ~ line returns</returns>
        public override string ToString()
        {
            //get column headings
            StringBuilder sb = new StringBuilder();
            sb.Append("RANK " + GetColumnHeadingsString(" ") + "~");
            //string table = "RANK " + GetColumnHeadingsString(" ") + "~";

            for (int i = 0; i < Entries; i++)
                sb.Append(GetRowStringRank(i, " ") + "~");
            //table += GetRowStringRank(i, " ") + "~";

            return sb.ToString();
            //return table;
        }
        /// <summary>
        /// Gets the entire score table in a formatted string ready for text diplay
        /// </summary>
        /// <param name="scoreFormatter">A call back function that processes the format of the score </param>
        /// <returns></returns>
        public string ToString(IntCallBack scoreFormatter)
        {
            //get column headings
            StringBuilder sb = new StringBuilder();
            sb.Append("RANK " + GetColumnHeadingsString(" ") + "~");
            //string table = "RANK " + GetColumnHeadingsString(" ") + "~";

            for (int i = 0; i < Entries; i++)
            {
                //pick up rank and score item
                string rank = (i + 1).ToString();
                sb.Append(rank.PadRight(6 - rank.Length));
                sb.Append(scoreFormatter((int)ItemValueAsObject(i, 0)) + " ");
                for (int c = 1; c < ColumnCount; c++)
                    sb.Append(ItemValueAsString(i, c) + " ");
                sb.Append("~");
            }
            //table += GetRowStringRank(i, " ") + "~";

            return sb.ToString();
            //return table;
        }
        /// <summary>
        /// gets a partially formatted table without the column headings
        /// This is useful for quick highscore displays in prototypes
        /// </summary>
        public string ToStringNoHeader
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < Entries; i++)
                    sb.Append(GetRowStringRank(i, " ") + "~");

                return sb.ToString();
            }
        }

        /// <summary>
        /// returns a simple list of the given number of top scores
        /// </summary>
        /// <param name="number">number of scores to return</param>
        /// <returns>Score list ready for drawing as text</returns>
        public string QuickList(int number)
        {
            StringBuilder sb = new StringBuilder();

            number = Math.Min(number, Entries);

            for (int i = 0; i < number; i++)
                sb.Append(GetRowStringRank(i, " ") + "~");

            return sb.ToString();
        }

        /// <summary>
        /// gets a simple list of all the scores available
        /// </summary>
        public string ToStringQuickList
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < Entries; i++)
                    sb.Append(GetRowStringRank(i, " ") + "~");

                return sb.ToString();
            }
        }
    }

    /// <summary>
    /// pre processor for score display when using standard ToString() arrangement
    /// This allows formatting of the score output
    /// </summary>
    /// <param name="score"></param>
    /// <returns></returns>
    public delegate string IntCallBack(Int32 score);

    /// <summary>
    /// pre processor for score display when using standard ToString() arrangement
    /// works with 64 bit integers
    /// This allows formatting of the score output
    /// </summary>
    /// <param name="score"></param>
    /// <returns></returns>
    public delegate string Int64CallBack(Int64 score);
}
