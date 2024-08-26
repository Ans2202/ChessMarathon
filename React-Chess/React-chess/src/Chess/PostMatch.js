import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { addMatch } from "../Service/ApiService";
import "../CSS/PostMatch.css"
import Navbar from "./Navbar";

const PostMatch=()=>{

    const [match, setMatch] = new useState({matchId:0, match1Id:0, match2Id:0, matchDate:'', matchLevel:'', winnerId: 0})
    const [loading,setLoading]=useState(true)
    const [error,setError]=useState(null)
    const navigate = useNavigate();
   
    const onChange = (e) => {
        setMatch({...match, [e.target.id] : e.target.value})
    }


    const createMatch = async () => {
        try {
            setLoading(true);
            const response = await addMatch(match);
            if (response.error) {
                throw new Error(response.error.message);
            }
            setLoading(false);
            console.log('Added the match : ' + response);
            navigate('/');  
        } catch (error) {
            setLoading(false);
            setError(error);
        }
    };
    const validate = (e) => {
        e.preventDefault();

        if(match.matchId==='')    alert("Id is Mandatory")
        else if (match.player1Id==='')    alert("Name is Mandatory")
        else if (match.player2Id==='')    alert("Team Id is Mandatory")
        else if (match.matchDate==='')    alert("MatchDate is Mandatory")
        else if (match.matchlevel==='')    alert("MatchLevel is Mandatory")
        else if (match.winnerId==='')    alert("Matches Played is Mandatory")
        else {
            console.log("Form submitted");
            createMatch();
    }

        
    }


    if(loading) {<h1>Loading</h1>}
    if(error) {<h1>{error.message}</h1>}
    return <>
        <Navbar/>
        <h1>Add a new Match</h1>
        <form className='form-group' onSubmit={validate}>
        <div>
                Id : <input className='form-control' value={match.matchId} onChange={onChange} type='number' id='matchId' />
            </div>
            <div>
                Player1Id : <input className='form-control' value={match.player1Id} onChange={onChange} type='number' id='player1Id' />
            </div>
            <div>
                Player2Id : <input className='form-control' value={match.player2Id} onChange={onChange} type='number' id='player2Id'/>
            </div>
            <div>
                MatchDate : <input className='form-control' value={match.matchDate} onChange={onChange} type='text' id='matchDate'/>
            </div>
            <div>
                MatchLevel : <input className='form-control' value={match.matchLevel} onChange={onChange} type='text' id='matchLevel' />
            </div>
            <div>
                winnerId : <input className='form-control' value={match.winnerId} onChange={onChange} type='number' id='winnerId'  />
            </div>
            <button className='btn btn-primary m-2 p-3' type='submit'>Add new Match</button>
        </form>
    </>

}

export default PostMatch;