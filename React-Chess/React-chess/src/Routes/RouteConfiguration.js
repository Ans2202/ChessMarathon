import { BrowserRouter, Route, Routes } from "react-router-dom"
import Home from "../Chess/Home"
import PostMatch from "../Chess/PostMatch"
import PlayerByCountry from "../Chess/PlayerByCountry"
import PlayerPerformance from "../Chess/PlayerPerformance"
import HighestWonPlayer from "../Chess/HighestWonPlayer"

const RouterConfig =()=>{
    return <>
        <BrowserRouter>
        
        <Routes>
            <Route path="/" element={<Home/>}/>
            <Route path="/addmatch" element={<PostMatch/>}/>
            <Route path="/playerbycountry" element={<PlayerByCountry/>}/>
            <Route path="/playerperformace" element={<PlayerPerformance/>}/>
            <Route path="/highestwonplayer" element={<HighestWonPlayer/>}/>

        </Routes>
        
        </BrowserRouter>
    </>
}

export default RouterConfig