using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchSpaceAIGame
{
    /// <summary>
    /// Defines the Graph, which is a tree of states connected by arcs. Can use DFS or BFS to find desired goal states.
    /// </summary>
    /// <typeparam name="TArc"></typeparam>
    class Graph<TArc> where TArc : ArcBase
    {
        const int NUMSTARTSTATES = 1;
        public List<StateBase<TArc>> StartStates;
        public List<StateBase<TArc>> AllStates;

        public Graph(){
            StartStates = new List<StateBase<TArc>>();
            AllStates = new List<StateBase<TArc>>();
        }

        /// <summary>
        /// Adds the state to the list of start states, preferrably an 'empty' state.
        /// </summary>
        /// <param name="st">The state to be used as a start state.</param>
        public void AddToStartStates(StateBase<TArc> st)
        {
            StartStates.Add(st);
            AllStates.Add(st);
        }

        public void InitStartStates()
        {
            if(StartStates.Count == NUMSTARTSTATES)
            {
                //Add a for loop if we are making games with multiple start states
                StartStates[0].SetToStartState();
            }
        }

        public void PrintAllStates()
        {
            Console.WriteLine("Printing all states...");
            for (int i = 0; i < AllStates.Count; i++)
            {
                AllStates[i].printdata();
            }
            Console.WriteLine("--Done with print--");
        }

        public bool IsInGraph(StateBase<TArc> st)
        {

            for(int i = 0; i < AllStates.Count; i++)
            {
                if(st != AllStates[i])
                {

                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        public void Reset()
        {
            StartStates[0].sending.Clear();
            AllStates.Clear();
        }


        /// <summary>
        /// Uses Breadth First search to find a goal state.
        /// </summary>
        /// <param name="verbose">True if you want to print state and arc data.</param>
        public void BreadthFirstSearch(bool verbose)
        {
            Queue<StateBase<TArc>> open = new Queue<StateBase<TArc>>();
            List<StateBase<TArc>> closed = new List<StateBase<TArc>>();

            Console.WriteLine("Using BFS to find the goal state............");
            open.Enqueue(StartStates[0]);
            
            while (open.Count > 0) //While open contains states
            {
                StateBase<TArc> st = open.Dequeue();//Get a state from open
                if (st.isGoal) //If it's a goal, we're done
                {
                    //Successfully found a goal state
                    Console.WriteLine("BFS Successfully found a goal state!");
                    int numMoves = 0;
                    while (st.receiving != null)
                    {
                        numMoves += 1;
                        if (verbose)
                        {
                            st.receiving.printdata();
                            st.printdata();
                        }
                        st = st.parent;
                    }
                    if (verbose)
                    {
                        StartStates[0].printdata();
                    }
                    Console.WriteLine("BFS took " + numMoves + " turns!");
                    Reset();
                    return;
                }
                else
                {
                    st.GenerateChildren();
                    //Double check if children are already in graph here

                    for (int i = 0; i < st.GetNumberChildren(); i++)
                    {

                        if (IsInGraph(st.sending[i]))
                        {
                            //Remove this child because its already in the graph

                            st.sending.RemoveAt(i);
                            i--;
                        }
                        else
                        {
                            //AllStates.Add(st.sending[0]);
                        }
                    }


                    closed.Add(st);

                    //Loop through children and see if they're in open or closed, then discard the child.

                    for (int i = 0; i < st.GetNumberChildren(); i++)
                    {

                        if (open.Contains(st.sending[i]) || closed.Contains(st.sending[i]))
                        {

                            //AllStates.Remove(st.sending[i]);
                            st.sending.RemoveAt(i);
                            i--;
                        }

                    }


                    //Pop the remaining to open
                    for (int i = 0; i < st.GetNumberChildren(); i++)
                    {
                        AllStates.Add(st.sending[i]);
                        open.Enqueue(st.sending[i]);
                    }
                }

            }
            //PrintAllStates();
            Console.WriteLine("BFS did not find the goal state...");
            Reset();
            //Was not successful here
        }

        /// <summary>
        /// Uses Depth First search to find a goal state for the game.
        /// </summary>
        /// <param name="verbose">True if you want to print states and move data.</param>
        public void DepthFirstSearch(bool verbose)
        {
            Stack<StateBase<TArc>> open = new Stack<StateBase<TArc>>();
            List<StateBase<TArc>> closed = new List<StateBase<TArc>>();

            Console.WriteLine("Using DFS to find the goal state............");
            open.Push(StartStates[0]);
            while (open.Count > 0)
            {
                StateBase<TArc> st = open.Pop();
                if (st.isGoal)
                {
                    //Successfully found a goal state
                    Console.WriteLine("DFS Successfully found a goal state!");
                    int numMoves = 0;
                    while(st.receiving != null)
                    {
                        numMoves += 1;
                        if (verbose)
                        {
                            st.receiving.printdata();
                            st.printdata();
                        }
                        st = st.parent;
                    }
                    if (verbose)
                    {
                        StartStates[0].printdata();
                    }
                    Console.WriteLine("DFS took " + numMoves + " turns!");
                    Reset();
                    return;
                }
                else
                {
                    st.GenerateChildren();
                    //Double check if children are already in graph here
                    
                    for(int i = 0; i < st.GetNumberChildren(); i++)
                    {
                        
                        if (IsInGraph(st.sending[i]))
                        {
                            //Remove this child because its already in the graph
                            
                            st.sending.RemoveAt(i);
                            i--;
                        }
                        else
                        {
                            //AllStates.Add(st.sending[0]);
                        }
                    }
                    

                    closed.Add(st);

                    //Loop through children and see if they're in open or closed, then discard the child.
                    
                    for (int i = 0; i < st.GetNumberChildren(); i++)
                    {
                        
                        if (open.Contains(st.sending[i]) || closed.Contains(st.sending[i]))
                        {
                            
                            //AllStates.Remove(st.sending[i]);
                            st.sending.RemoveAt(i);
                            i--;
                        }
                        
                    }
                    

                    //Pop the remaining to open
                    for (int i = 0; i < st.GetNumberChildren(); i++)
                    {
                        AllStates.Add(st.sending[i]);
                        open.Push(st.sending[i]);
                    }
                }

            }
            //PrintAllStates();
            Console.WriteLine("DFS did not find the goal state...");
            Reset();
            //Was not successful here
        }
        //bool hasgenerated start states;
        //DFS
        //BFS
        //RemoveRedundantChildren(TState state)
        //GenerateStartStates
    }


   
}
