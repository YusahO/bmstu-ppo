import AllAudiotracks from "../components/audiotrack/AllAudiotracks";
import UpperPanel from "./UpperPanel.jsx";

const Home = () => {
  return (
    <div style={{ padding: '20px' }}>
      <UpperPanel displayFunctional={true}/>
      <div style={{ paddingTop: '50px' }}>
        <AllAudiotracks />
      </div>
    </div>
  )
};

export default Home;