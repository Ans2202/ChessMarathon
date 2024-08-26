import { useState, useEffect } from "react"
import { GetPlayerPerformance} from "../Service/ApiService"
import "../CSS/PlayerPerformance.css"
import Navbar from "./Navbar"

const PlayerPerformance=()=>{

    const [data,setData]=useState([])
    const [loading,setLoading]=useState(true)
    const [error,setError]=useState(null)

    useEffect(()=>{
        const fetchData=async ()=>{
            const response= await GetPlayerPerformance()
        setData(response.data)
        setLoading(response.loading)
        setError(response.error)
        }

        fetchData();
    },[])

    if(loading) return <p>Loading</p>
    if(error) return <p>Error: {error.message}</p>

    return <>
    <Navbar/>
    {data.length > 0 ? (
    <div>
        <h1>Player Performance</h1>
        <table>
            <thead>
                <tr>
                    <th>Sr No.</th>
                    <th>Name</th>
                    <th>Total Match Played</th>
                    <th>Total Win Count</th>
                    <th>Win Percentage</th>
                </tr>
            </thead>
            <tbody>
                {data.map((player, key) => (
                    <tr key={key}>
                        <td>{key+1}</td>
                        <td>{player.fullName}</td>
                        <td>{player.totalMatchesPlayed}</td>
                        <td>{player.totalMatchesWon}</td>
                        <td>{player.winPercentage}</td>
                    </tr>
                ))}
            </tbody>
        </table>
    </div>
) : (
    <p>No stats available</p>
)}

    </>


}
export default PlayerPerformance;