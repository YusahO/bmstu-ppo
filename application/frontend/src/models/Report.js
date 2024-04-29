import { v4 as uuidv4 } from "uuid";
import { ReportStatus } from "./enums/ReportStatus.js";

const Report = {
  id: uuidv4(),
  authorId: uuidv4(),
  audiotrackId: uuidv4(),
  text: "",
  status: ReportStatus(),
};

export default Report;