import axios from "axios";

const URL='http://localhost:5087/api/Chess/player'

const addMatch = async (match) => {
    try {
        const response = await axios.post(URL, match);
        return { data: response.data, loading: false, error: null };
    } catch (error) {
        return { data: null, loading: false, error: error };
    }
}

const GetPlayerByCountry=async (country)=>{
    let url=`${URL}/ByCountry`
    try {
        const response = await axios.get(url,{
            params: {
                country: country
            }
        });
        return { data: response.data, loading: false, error: null };
    } catch (error) {
        return { data: null, loading: false, error: error };
    }
}

const GetPlayerPerformance=async ()=>{
    let url=`${URL}/playerPerformances`
    try {
        const response = await axios.get(url);
        return { data: response.data, loading: false, error: null };
    } catch (error) {
        return { data: null, loading: false, error: error };
    }
}

const GetHighestWonPlayer = async () => {
    let url=`${URL}/topPlayerByWinPercentage`
    try {
        const response = await axios.get(url);
        console.log(response.data)
        return { data: response.data, loading: false, error: null };
    } catch (error) {
        return { data: null, loading: false, error: error };
    }
};

export {addMatch, GetPlayerByCountry, GetPlayerPerformance, GetHighestWonPlayer}