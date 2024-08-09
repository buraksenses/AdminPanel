import Modal from "../components/Modal";
import Grid from "../components/Grid";
import { useConfig } from "../Contexts/ConfigurationsContext";
import "../ConfigurationPage.css";

const ConfigurationPage = () => {
  const { showModal } = useConfig();

  return (
    <div className="configuration-page">
      <h1>Building Configuration</h1>
      <Grid />
      {showModal && <Modal />}
    </div>
  );
};

export default ConfigurationPage;
