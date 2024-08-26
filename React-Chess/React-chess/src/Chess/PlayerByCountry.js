import { useState } from 'react';
import { GetPlayerByCountry } from '../Service/ApiService';
import '../CSS/PlayerByCountry.css'; // Import the CSS file
import Navbar from './Navbar';

const PlayerByCountry = () => {
    const [players, setPlayers] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [country, setCountry] = useState('');
    const [submit, setSubmit] = useState(false);

    const fetchPlayers = async () => {
        try {
            setLoading(true);
            const response = await GetPlayerByCountry(country);
            setPlayers(response.data);
            setLoading(false);
            setSubmit(true);
            setCountry('');
        } catch (err) {
            setError(err);
            setLoading(false);
        }
    };

    const handleSubmit = () => {
        if (country === '' ) {
            alert("Country Name is required.");
        } else {
            fetchPlayers();
        }
    };

    if (loading) return <p className="loading">Loading...</p>;
    if (error) return <p className="error">Error: {error.message}</p>;

    return (
        <><Navbar/>
        <div className="container">
            <input
                value={country}
                onChange={(e) => setCountry(e.target.value)}
                type="text"
                id="country"
                placeholder="Enter Country Name"
            />
            
            <button onClick={handleSubmit} type="button">Enter</button>

            {submit ? (
                <div>
                    <h1>Player Details</h1>
                    {players.length > 0 ? (
                        <table className="table">
                            <thead>
                                <tr>
                                    <th>Player Id</th>
                                    <th>Player Name</th>
                                    <th>Country</th>
                                    <th>Current World Ranking</th>
                                    <th>Total Match Played</th>
                                </tr>
                            </thead>
                            <tbody>
                                {players.map((player, index) => (
                                    <tr key={index}>
                                        <td>{player.playerId}</td>
                                        <td>{player.playerName}</td>
                                        <td>{player.country}</td>
                                        <td>{player.currentWorldRanking}</td>
                                        <td>{player.totalMatchesPlayed}</td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    ) : (
                        <p>No players found for the selected country.</p>
                    )}
                </div>
            ) : (
                <p>Data will be displayed here..</p>
            )}
        </div>
        </>
    );
};

export default PlayerByCountry;
