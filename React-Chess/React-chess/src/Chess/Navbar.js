import "../CSS/Navbar.css"
import { Link } from "react-router-dom"

const Navbar=()=>{
    return <>
    
    <nav>
        <h1>Chess Marathon</h1>
        <li>
            <Link to='/addmatch'>Add Match</Link>
        </li>
        <li>
        <Link to='/playerbycountry'>PlayerByCountry</Link>
        </li>
        <li>
        <Link to='/playerperformace'>PlayerPerformance</Link>
        </li>
        <li>
        <Link to='/highestwonplayer'>HighestWonPlayer</Link>
        </li>
    </nav>
    
    </>
}

export default Navbar