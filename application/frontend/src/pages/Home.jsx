import AllAudiotracks from "../components/audiotrack/AllAudiotracks";

const Home = () => {
  return (
    <div style={{ display: 'flex', alignItems: 'stretch' }}>
      <div style={{ width: '100%' }}>
        <AllAudiotracks renderAdd={true} />
      </div>
    </div>
  )
};

export default Home;