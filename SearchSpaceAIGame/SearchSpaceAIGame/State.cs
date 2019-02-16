using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchSpaceAIGame
{
    /// <summary>
    /// This class contains references to its children, parent, and the arc from its parent. The class contains a state of the game, and whether it is a goal state.
    /// </summary>
    /// <typeparam name="TArc">The type of Arc's you are using to represent change in state.</typeparam>
    public abstract class StateBase<TArc> where TArc : ArcBase
    {
        public TArc receiving;
        public StateBase<TArc> parent;
        public List<StateBase<TArc>> sending;
        public bool isGoal;
        public bool isDead;

        public static bool operator ==(StateBase<TArc> st, StateBase<TArc> other)
        {
            return false;
        }


        public static bool operator !=(StateBase<TArc> st, StateBase<TArc> other)
        {
            return true;
        }

        /// <summary>
        /// Generates All children of this state
        /// </summary>
        /// <returns></returns>
        public abstract bool GenerateChildren();

        /// <summary>
        /// Generates a state from this state using the given arc
        /// </summary>
        /// <param name="arc">The move data from this state to the new state</param>
        /// <returns></returns>
        public abstract bool GenerateState(TArc arc);
        public abstract StateBase<TArc> GetLeftChild();
        public abstract StateBase<TArc> GetRightChild();
        public abstract int GetNumberChildren();

        /// <summary>
        /// This function is intended to check if a state is valid.
        /// </summary>
        /// <returns>Validity of the state</returns>
        public abstract bool VerifyValid();

        /// <summary>
        /// Checks if this state is a goal state for the game.
        /// </summary>
        /// <returns>True if is a desired goal state</returns>
        public abstract bool VerifyGoal();

        /// <summary>
        /// This is used to initalize the start state's data.
        /// </summary>
        /// <returns>If the initialize was done successfully</returns>
        public abstract bool SetToStartState();

        /// <summary>
        /// Outputs the state data.
        /// </summary>
        public abstract void printdata();
    }

    class HanoiState : StateBase<HanoiArc>
    {
        public const int NUMPIECES = 4;
        public const int NUMTOWERS = 3;
        public int[,] StateData;

        public HanoiState()
        {
            StateData = new int[NUMTOWERS, NUMPIECES];
            this.sending = new List<StateBase<HanoiArc>>();
            //Ensure all data is set to zero
            for(int x = 0; x < NUMTOWERS; x++)
            {
                for(int y = 0; y < NUMPIECES; y++)
                {
                    StateData[x, y] = 0;
                }
            }
        }

        public override bool Equals(Object other)
        {
            if(other.GetType() == typeof(HanoiState))
            {
                return ((HanoiState)other == this);
            }
            return false;
        }

        public static bool operator ==(HanoiState st, HanoiState other)
        {
            for(int x = 0; x < NUMTOWERS; x++)
            {
                for(int y = 0; y < NUMPIECES; y++)
                {
                    if(st.StateData[x,y] != other.StateData[x, y])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool operator !=(HanoiState st, HanoiState other)
        {
            return !(st == other);
        }

        public override int GetHashCode()
        {
            int sum = 0;
            for (int x = 0; x < NUMTOWERS; x++)
            {
                for (int y = 0; y < NUMPIECES; y++)
                {
                    sum += StateData[x, y];
                }
            }
            return sum;
        }

       /// <summary>
       /// Generates all the children of this state. Checks if the states are valid and if they are goal states.
       /// </summary>
       /// <returns>True if the function was able to generate children.</returns>
        public override bool GenerateChildren()
        {
            //Generate arcs for how many possible moves there are
            List<HanoiArc> possibleMoves = new List<HanoiArc>();
            for (int x = 0; x < NUMTOWERS; x++)
            {
                if (this.StateData[x,0] != 0)
                {
                    for(int i = 0; i < NUMTOWERS; i++)
                    {
                        if (x != i)
                        {
                            HanoiArc temp = new HanoiArc(x,i);
                            possibleMoves.Add(temp);
                        }
                        
                    }
                }
            }
            //---------------




            //Use those arcs to generate new states with GenerateState
            for(int i = 0; i < possibleMoves.Count; i++)
            {
                GenerateState(possibleMoves[i]);
            }

            //Check if valid
            for(int i = 0; i < GetNumberChildren(); i++)
            {
                if (!this.sending[i].VerifyValid())
                {
                    this.sending.RemoveAt(i);
                    i = 0;
                }
            }

            //-------------------
            //---------------------------
            return true;
        }

        /// <summary>
        /// Generates a new state from an arc with the move data.
        /// </summary>
        /// <param name="arc">This arc only contains the move data. The sending pointer will be changed.</param>
        /// <returns></returns>
        public override bool GenerateState(HanoiArc arc)
        {
            HanoiState st = new HanoiState();
            st.receiving = arc;
            st.parent = this;
            //copy state data from this to st
            for(int x = 0; x < NUMTOWERS; x++)
            {
                for(int y = 0; y < NUMPIECES; y++)
                {
                    st.StateData[x, y] = this.StateData[x, y];
                }
            }
            //---------

            //Move Pieces
            int movingPieceX = arc.sourceTower;
            int movingPieceY = 0;

            //Find the moving pieces location
            for(int y = 0; y < NUMPIECES; y++)
            {
                if (y == NUMPIECES - 1 && st.StateData[movingPieceX, y] != 0)
                {
                    movingPieceY = NUMPIECES - 1;
                    y = NUMPIECES;
                    break;
                    
                }

                if (st.StateData[movingPieceX, y] == 0 && y >= 1)
                {
                    movingPieceY = y - 1;
                    break;
                }

                
            }
            //------------------------------


            //Find the destination location
            int destX = arc.destTower;
            int destY = 0;
            for(int y = 0; y < NUMPIECES; y++)
            {
                if (y == NUMPIECES - 1 && st.StateData[movingPieceX, y] != 0)
                {
                    return false;
                }
                if (st.StateData[destX, y] == 0)
                {
                    destY = y;
                    y = NUMPIECES;
                }
            }
            //----------------------------


            //Now swap the pieces
            int temp = st.StateData[destX,destY];
            st.StateData[destX, destY] = st.StateData[movingPieceX, movingPieceY];
            st.StateData[movingPieceX, movingPieceY] = temp;
            //---------------------


            //Now Check if valid
            if (st.VerifyValid())
            {
                this.sending.Add(st);
                st.VerifyGoal();
                return true;
            }
            else
            {
                return false;
            }
            //-----
            //-------------
        }


        public override bool VerifyValid()
        {
            bool[] pieces = new bool[NUMPIECES];
            for(int x = 0; x < NUMTOWERS; x++)
            {
                for(int y = 0; y < NUMPIECES; y++)
                {
                    if (this.StateData[x, y] != 0)
                    {
                        //If we reach a piece that is already accounted for
                        if (pieces[this.StateData[x, y] - 1] == true)
                        {
                            //We have a redundant piece
                            return false;
                        }
                        else
                        {
                            //Otherwise, indicate this piece has been accounted for
                            pieces[this.StateData[x, y] - 1] = true;
                        }

                        if (y > 0 && this.StateData[x, y] >= this.StateData[x, y - 1])
                        {
                            return false;
                        }
                    }
                    else { }
                }
            }


            //Makes sure that no game piece is unaccounted for.
            for(int i = 0; i < NUMPIECES; i++)
            {
                if(pieces[i] == false)
                {
                    return false;
                }
            }

            return true;
        }
        
        /// <summary>
        /// Checks if this state is a goal state and changes it's IsGoal bool accordingly.
        /// </summary>
        /// <returns></returns>
        public override bool VerifyGoal()
        {
            for(int y = 0; y < NUMPIECES; y++)
            {
                if(this.StateData[NUMTOWERS - 1,y] != NUMPIECES - y)
                {
                    return false;
                }
            }

            if (VerifyValid())
            {
                this.isGoal = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override StateBase<HanoiArc> GetLeftChild()
        {
            if (this.sending.Count > 0)
            {
                return this.sending[0];
            }
            else return null;
        }

        public override StateBase<HanoiArc> GetRightChild()
        {
            if (this.sending.Count > 0)
            {
                return this.sending[sending.Count - 1];
            }
            else return null;
        }

        public override int GetNumberChildren()
        {
            return this.sending.Count;
        }

        public override bool SetToStartState()
        {
            this.receiving = null;
            for(int x = 0; x < NUMTOWERS; x++)
            {
                for(int y = 0; y < NUMPIECES; y++)
                {
                    if(x == 0)
                    {
                        this.StateData[x, y] = NUMPIECES - y;
                    }
                    else
                    {
                        this.StateData[x, y] = 0;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Prints the data inside the state.
        /// </summary>
        public override void printdata()
        {
            for(int y = NUMPIECES - 1; y >= 0; y--)
            {
                for(int x = 0; x < NUMTOWERS; x++)
                {
                    Console.Write(this.StateData[x, y]);
                }
                Console.WriteLine("|");
            }
            Console.WriteLine("----");
        }
    }


  


}
