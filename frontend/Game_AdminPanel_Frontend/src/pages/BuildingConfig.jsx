import "../ConfigurationPage.css";
import Modal from "../components/Modal";
import Grid from "../components/Grid";
import { useConfig } from "../Contexts/ConfigurationsContext";
import Confetti from "react-confetti";

const ConfigurationPage = () => {
  const { showModal, confetti } = useConfig();

  return (
    <div className="configuration-page">
      <h1>Building Configuration</h1>
      <Grid />
      {showModal && <Modal />}
        {confetti && <Confetti />}
    </div>
  );
};

export default ConfigurationPage;
