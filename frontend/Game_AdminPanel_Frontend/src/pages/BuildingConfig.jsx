import "../ConfigurationPage.css";
import Modal from "../components/Modal";
import Grid from "../components/Grid";
import { useConfig } from "../Contexts/ConfigurationsContext";

const ConfigurationPage = () => {
  const { setModalType, setShowModal, showModal } = useConfig();

  return (
    <div className="configuration-page">
      <h1>Building Configuration</h1>
      <Grid />
      <button
        className="add-button"
        onClick={() => {
          setModalType("add");
          setShowModal(true);
        }}
      >
        Add Configuration
      </button>
      {showModal && <Modal />}
    </div>
  );
};

export default ConfigurationPage;
